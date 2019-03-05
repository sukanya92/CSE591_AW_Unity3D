using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarChart : MonoBehaviour {

	public float[] getValues;
    public Bar barPrefab;
    List<Bar> bars = new List<Bar>();
    float chartHeight;
    RectTransform[] rect = new RectTransform[3]; 
    bool b = true;
    float interval = 0.5f; 
    float nextTime = 0;
    int k = 0;
    public Color[] colors = {new Color(1, 0, 1, 1), new Color(0,1,0,1), new Color(0,0,1,1)};
    float[] values = {0.5f, 1f, 1f};
    private RectTransform barChart;
    float yMax = 1.0f;
    float factor = 0f;
    float tempX, tempY, tempZ;
    
    // Use this for initialization
	void Start () {
        //chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;
        //barChart = transform.Find ("Bar Chart").GetComponent<RectTransform>();
        chartHeight = 147f;
        factor = chartHeight/yMax;
		//float[] values = {-0.1f, 0.2f, 0.7f};
        DisplayGraph(values);   
	}
	
	// Update is called once per frame
	
    void Update () {
     if (Time.time >= nextTime) {
        
         
        tempX = Input.acceleration.x;
        tempY = Input.acceleration.y;
        tempZ = Input.acceleration.z;
         
        //Debug.Log("Acceleration "+tempX+" "+tempY+" "+tempZ);
        rect[0].localScale = new Vector3(3f, (tempX*factor), 1.0f);
        rect[1].localScale = new Vector3(3f, (tempY*factor), 1.0f);
        rect[2].localScale = new Vector3(3f, (tempZ*factor), 1.0f); 
         
       
         
         
         
         
        /* 
         
         
         rect[0].localScale = new Vector3(5f, .20f, 1.0f); 
        //rect[0].localPosition += new Vector3(0.0f, //k*k,0.0f);//Vector3.up*20; 
        
        values =  getValues;
        Debug.Log("=="+rect[0].localScale.y);
        //rect[0].localScale = new Vector3(0.5f, values[0], 1.0f);
        //rect[1].localScale = new Vector3(0.5f, values[1], 1.0f);
        //rect[2].localScale = new Vector3(0.5f, values[2], 1.0f);
         
       if(b){
            rect[0].localScale = new Vector3(3f, (1.0f*factor), 1.0f);
            rect[1].localScale = new Vector3(3f, (1.0f*factor), 1.0f);
            rect[2].localScale = new Vector3(3f, (1.0f*factor), 1.0f);
            b = false;
        }else{
             rect[0].localScale = new Vector3(3f, (-1.0f*factor), 1.0f);
            rect[1].localScale = new Vector3(3f, (-1.0f*factor), 1.0f);
            rect[2].localScale = new Vector3(3f, (-1.0f*factor), 1.0f);
            b = true;
        }
        
        */
        
        //k++;
        nextTime += interval; 
      }
       
	}
    
    void DisplayGraph(float[] vals){
        for(int i=0;i<vals.Length;i++){
            Bar newBar = Instantiate(barPrefab) as Bar;
            newBar.transform.SetParent(transform);
            RectTransform rt = newBar.bar.GetComponent<RectTransform>();
            rt.localScale = new Vector3(0.5f, 50.0f, 1.0f);
            rt.localPosition += Vector3.up;
            
            newBar.bar.color = colors[i];
            rect[i] = rt;
            
        }
    }
    
}
