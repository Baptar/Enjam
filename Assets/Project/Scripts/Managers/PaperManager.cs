using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperManager : MonoBehaviour
{
   // TODO : APPEAR PAPER CALL ON THE CORRESPONDING PAPER "APPEAR" -> REF ON EACH PAPER
   // TODO : INTERACT WITH PAPER ASSIGN ITSELF HERE,
   // TODO : WHEN "REMOVE PAPER" THEN CALL ON THE PAPER "ON REMOVED"
   
   
   public enum EPaperEvent
   {
      None,
      Candy,
      ParcFell,
      ChillBeer,
      WatchTV,
   }
   private EPaperEvent paperEvent = EPaperEvent.None;

   private void Start()
   {
      // TODO : deactivate each paper
   }
   
   // TODO
   public void AppearPaper(EPaperEvent paper)
   {
      switch (paper)
      {
         case EPaperEvent.Candy:
            break;
         
         case EPaperEvent.ParcFell:
            break;
         
         case EPaperEvent.ChillBeer:
            break;
         
         case EPaperEvent.WatchTV:
            break;
      }
   }
   

   public void RemovePaper()
   {
      switch (paperEvent)
      {
         case EPaperEvent.Candy:
            break;
         
         case EPaperEvent.ParcFell:
            break;
         
         case EPaperEvent.ChillBeer:
            break;
         
         case EPaperEvent.WatchTV:
            break;
      }
   }
}
