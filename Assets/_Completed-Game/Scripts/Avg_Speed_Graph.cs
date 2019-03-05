using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Avg_Speed_Graph : MonoBehaviour {
	   [SerializeField] private Sprite circleSprite;
	   private RectTransform graphContainer;
       float interval = 0.5f; 
       float nextTime = 0;
       int k=0;
       bool t = false;
       float ht = 0f;
       float unitWidth = 0f;
       float yMaximum = 12; 
       float xMaximum = 9;
       float xSize = 42;
       float ySize = 30;
       bool updt = false;
       public static int countSession = -1; 
       public static float avgSpeed = 0;
       private static List<float> valueList = new List<float>() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
       private static Queue<float> myQueue = new Queue<float>();
       public static bool flag = true;
       public static bool inAwake = true;
    
    
    private void Awake(){
       
         graphContainer = transform.Find ("graphContainer").GetComponent<RectTransform>();
        if(!flag){
            //Debug.Log("====INSIDE AWAKE====");
         
            if(countSession>9){
                myQueue.Dequeue();  
            }
            
            if(countSession>=0){
                //Debug.Log("Avg Speed is "+avgSpeed); 
                myQueue.Enqueue(avgSpeed);
                //Debug.Log("Enqued "+avgSpeed); 
            } 
            flag = true;
        }
        
        float[] arr = myQueue.ToArray(); 
        
        foreach(float l in arr) 
        { 
            //Debug.Log("***"+l); 
        } 
        
        
        //Update List
        for(int i=0;i<arr.Length;i++){
            valueList[i] = arr[i];
        }
        
        
        
        for(int j=0;j<valueList.Count;j++){
            //Debug.Log("ValueList "+j+" - "+valueList[j]+" ");
        }
        
       showGraph(valueList);
       updt = true;    
	}
    
    private void Update(){

    }
   
	private GameObject createCircle(Vector2 anchoredPosition){
		GameObject gameObject = new GameObject("circle", typeof(Image));
		gameObject.transform.SetParent(graphContainer, false);
		gameObject.GetComponent<Image>().sprite = circleSprite;
		RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = anchoredPosition;
		rectTransform.sizeDelta = new Vector2(11, 11);
		rectTransform.anchorMin = new Vector2(0, 0);
		rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
	}
    
    private void showGraph(List<float> valueList){
        float graphHeight = graphContainer.sizeDelta.y;
        ht = graphHeight-78;
        float graphWidth = graphContainer.sizeDelta.x;
        unitWidth = (graphWidth-110)/xMaximum;
        GameObject lastCircleGameObject = null;
        for(int i=0;i<valueList.Count;i++){
            float xPosition = xSize+(unitWidth*i);//(graphWidth)*(i/xMaximum);
            float yPosition = ySize+(valueList[i]/yMaximum)*ht;//(graphHeight-70);
            GameObject circleGameObject = createCircle(new Vector2(xPosition, yPosition));
            
            RectTransform rt = circleGameObject.GetComponent<RectTransform>();
            rt.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            
            if(lastCircleGameObject!=null){
                createDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, false, 0);}
            ///x[i] = circleGameObject;
            lastCircleGameObject = circleGameObject;
        }
    }
		
    private void createDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, bool b, int n){
        GameObject gameObject = new GameObject("doctConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1,1,1,.5f);
        RectTransform rectTransform =  gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
		rectTransform.anchorMin = new Vector2(0, 0);
		rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.anchoredPosition = dotPositionA + dir*distance*.5f;
        rectTransform.localEulerAngles = new Vector3(0,0,UtilsClass.GetAngleFromVectorFloat(dir));
  }
}
