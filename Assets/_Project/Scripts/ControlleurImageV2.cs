using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.AugmentedImage;
using GoogleARCore;

public class ControlleurImageV2 : MonoBehaviour
{
    public GameObject VisualizerPrefab;

    public Vector3 instantiateOffset;

    public GameObject FitToScanOverlay;

    private Dictionary<int, GameObject> m_Visualizers =
        new Dictionary<int, GameObject>();

    private List<AugmentedImage> m_TempUpdatedImages = new List<AugmentedImage>();

    // Update is called once per frame
    void Update()
    {
        //Quitte l'application quand le bouton retour est pressé
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }    

        //Vérifie que l'appli est en train de tracker
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        //Stocke dans m_TempUpdatedImages les informations de tracking des images 
        //présentent dans la BDD si elles sont détectées par ARCore via la caméra
        Session.GetTrackables<AugmentedImage>(m_TempUpdatedImages, TrackableQueryFilter.Updated);

        //Créé des visualiseurs et des ancres pour les AugmentedImage qui sont trackées
        //et qui ne disposent pas déjà de visualiseurs. Efface les visualiseurs des images plus trackées
        GameObject visualizer = null;
        foreach (var image in m_TempUpdatedImages)
        {
            visualizer = null;
            m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
            if (image.TrackingState == TrackingState.Tracking && visualizer == null)
            {
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                visualizer = Instantiate(VisualizerPrefab, anchor.transform );
                visualizer.transform.position += instantiateOffset;
                //visualizer.Image = image;
                m_Visualizers.Add(image.DatabaseIndex, visualizer);
            }
            else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
            {
                m_Visualizers.Remove(image.DatabaseIndex);
                GameObject.Destroy(visualizer.gameObject);
            }
        }

        //Affiche l'overlay fit-to-scan si il n'y a pas d'image en Tracking

        if (visualizer != null)
        {
            FitToScanOverlay.SetActive(false);
            return;
        }
   
        FitToScanOverlay.SetActive(true);
    }
}
