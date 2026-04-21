using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaperManager : MonoBehaviour
{
   // TODO : APPEAR PAPER CALL ON THE CORRESPONDING PAPER "APPEAR" -> REF ON EACH PAPER
   // TODO : INTERACT WITH PAPER ASSIGN ITSELF HERE,
   // TODO : WHEN "REMOVE PAPER" THEN CALL ON THE PAPER "ON REMOVED"
   
   
   [SerializeField] private PaperInteract paperCandy;
   [SerializeField] private PaperInteract paperWatchTV;
   [SerializeField] private PaperInteract paperChillBear;
   [SerializeField] private PaperInteract paperUnderstandParc;
   [SerializeField] private PaperInteract paperYellAtParc;
   
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
   }
   
   public void AppearPaperCandy() => paperCandy.MakePaperAppear();
   public void AppearPaperJudas() => paperWatchTV.MakePaperAppear();
   public void AppearPaperChillBeer() => paperChillBear.MakePaperAppear();
   public void AppearPaperUnderstandParc() => paperUnderstandParc.MakePaperAppear();
   public void AppearPaperParcFell() => paperYellAtParc.MakePaperAppear();
   

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
