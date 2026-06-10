using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PaperManager : MonoBehaviour
{
   [Header("Paper ref")]
   [SerializeField] private PaperInteract paperCandy;
   [SerializeField] private PaperInteract paperWatchTV;
   [SerializeField] private PaperInteract paperChillBear;
   [SerializeField] private PaperInteract paperUnderstandParc;
   [SerializeField] private PaperInteract paperYellAtParc;
   [SerializeField] private PaperInteract[] paperThrowRadio;
   
   [Space(10)]
   [Header("Parameters")]
   [SerializeField] private Ease easeLookAtPoint;
   
   private PaperInteract currentPaperInteract;

   private void Start()
   {
      paperCandy.SetInteractable(false);
      paperCandy.gameObject.SetActive(false);
      
      paperWatchTV?.SetInteractable(false);
      paperWatchTV?.gameObject.SetActive(false);
      
      paperChillBear?.SetInteractable(false);
      paperChillBear?.gameObject.SetActive(false);
      
      paperUnderstandParc?.SetInteractable(false);
      paperUnderstandParc?.gameObject.SetActive(false);
      
      paperYellAtParc?.SetInteractable(false);
      paperYellAtParc?.gameObject.SetActive(false);

      foreach (PaperInteract paper in paperThrowRadio)
      {
         paper?.SetInteractable(false);
         paper?.gameObject.SetActive(false);
      }
   }
   
   public void AppearPaperCandy() => paperCandy.MakePaperAppear(easeLookAtPoint);
   public void AppearPaperJudas() => paperWatchTV.MakePaperAppear(easeLookAtPoint);
   public void AppearPaperChillBeer() => paperChillBear.MakePaperAppear(easeLookAtPoint);
   public void AppearPaperUnderstandParc() => paperUnderstandParc.MakePaperAppear(easeLookAtPoint);
   public void AppearPaperParcFell() => paperYellAtParc.MakePaperAppear(easeLookAtPoint);
   

   public void RemovePaper()
   {
      if (!currentPaperInteract) return;
      
      currentPaperInteract.MakePaperDisappear();
      currentPaperInteract = null;
   }

   public void SetCurrentPaperRead(PaperInteract paper)
   {
      
      currentPaperInteract = paper;
   }
}
