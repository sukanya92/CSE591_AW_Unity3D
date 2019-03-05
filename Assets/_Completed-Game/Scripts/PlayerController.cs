using UnityEngine;
using System.Data;  // Added by ABhinethri for sqlite
using Mono.Data.Sqlite;
using System.Collections.Generic;

// Include the namespace required to use Unity UI
using UnityEngine.UI;
using System.Collections;



public class PlayerController : MonoBehaviour {

    // To calculate average speed
    public int count_speed;
    public float average_speed;
	
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public Text countText;
    public Text TutorialText;
	public Text winText;
    public string[] learn2 = new string[] { "Object Oriented Concepts",
        "Abstraction: Abstraction is the concept of hiding the internal details and describing things in simple terms.",
        "Encapsulation is the technique used to implement abstraction in object oriented programming. Encapsulation is used for access restriction to a class members and methods." ,
        "Access modifier keywords are used for encapsulation in object oriented programming. For example, encapsulation in java is achieved using private, protected and public keywords.",
        "Polymorphism is the concept where an object behaves differently in different situations. There are two types of polymorphism – compile time polymorphism and runtime polymorphism.",
        "Compile time polymorphism is achieved by method overloading. Runtime polymorphism is implemented when we have “IS-A” relationship between objects. ",
        "Inheritance is the object oriented programming concept where an object is based on another object. ",
        "Inheritance is the mechanism of code reuse. The object that is getting inherited is called superclass and the object that inherits the superclass is called subclass.",
        "Association is the OOPS concept to define the relationship between objects. Association defines the multiplicity between objects. ",
        "Aggregation is a special type of association. In aggregation, objects have their own life cycle but there is an ownership.",
        "Aggregation: Whenever we have “HAS-A” relationship between objects and ownership then it’s a case of aggregation.",
        "Composition is a special case of aggregation. Composition is a more restrictive form of aggregation.",
         "Compostion: When the contained object in “HAS-A” relationship can’t exist on it’s own, then it’s a case of composition."};



    // Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
    private Rigidbody rb;
	public static int count;
    public float speed2 = 5000f;
    private static int time = 0;
    private  int attempt = 0;
    public List<double> speed_list = new List<double>();
    public List<int> count_list = new List<int>();

    private double avgspeed = 5.0;
    Vector3 res;
    // At the start of the game..
    void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();
        res = GameObject.Find("Player").transform.position;

        Debug.Log("position= "+res[0] + "  " + res[1]);

        // Set the count to zero 
        count = 0;


		// Run the SetCountText function to update the UI (see below)
		SetCountText ();

		// Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
		winText.text = "";
     




        // Added by Abhinethri for sqlite
        string conn = "URI=file:" + Application.dataPath + "/plugins/ballgame.db"; // path to database
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        // Reading and icrementing attempt value
        string sqlQuery = "select max(attempt) from count1";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            attempt = reader.GetInt32(0) + 1;
        }
        // Debug.Log("attempt= " + attempt);
        reader.Close();
        reader = null;
 
        // Adding average speed
        string speed_query = "select avgspeed from avg_speed order by attempt desc limit 10";
             dbcmd.CommandText = speed_query;
        IDataReader reader1 = dbcmd.ExecuteReader();
       
        while (reader1.Read())
        {
            speed_list.Add(reader1.GetDouble(0));
           // Debug.Log("average speed= "+reader1.GetDouble(0));
        }
        reader1.Close();
        reader1 = null;


        // Adding count to list
        string count_query = "select count from count1 order by attempt desc limit 10";
        dbcmd.CommandText = count_query;
        IDataReader reader2 = dbcmd.ExecuteReader();

        while (reader2.Read())
        {
            count_list.Add(reader2.GetInt32(0));
          //  Debug.Log("count= "+reader2.GetInt32(0));
        }
        reader2.Close();
        reader2 = null;



        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        //code jp





        // Added by Abhinethri for sqlite
    }

    // Each physics step..
    void FixedUpdate ()
	{
       
		// Set some local float variables equal to the value of our Horizontal and Vertical Inputs
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		// Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
		//Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		// Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
		// multiplying it by 'speed' - our public player speed that appears in the inspector
		//rb.AddForce (movement * speed);
        Vector3 movement = new Vector3(Input.acceleration.x, 0f, Input.acceleration.y);
        rb.AddForce(movement * speed2 * Time.deltaTime);


        // To calculate average speed
        average_speed = average_speed + (movement[0] * speed2 * Time.deltaTime); // taking x-axis value
        count_speed++;
        // average_speed/count_speed -> while storing in queue

        //code jp
        res = GameObject.Find("Player").transform.position;
        //condition for testing the ball has crossed boundaries
        if (res[0]<-10.00 || res[0] > 10.00 || res[1] < -10.00 || res[1]>10.00 || res[2] < -10.00 || res[2] > 10.00)
        {
            //Set value
            if(Window_Graph.flag){
                float avgSpeed1 = average_speed/count_speed;
                Debug.Log("AVGSPEED1 "+avgSpeed1);
                Debug.Log("====FIXUPDATE====");
                //Debug.Log("incrementingg ...countSession ");
                Window_Graph.hitCount = count;
                Window_Graph.countSession++;
                Window_Graph.flag = false;
                
                Avg_Speed_Graph.avgSpeed = Mathf.Abs(avgSpeed1);
                Avg_Speed_Graph.countSession++;
                Avg_Speed_Graph.flag = false;
                
                
            }
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("aftergame");


            string con_count = "URI=file:" + Application.dataPath + "/plugins/ballgame.db"; // path to database
            SqliteConnection con2 = new SqliteConnection(con_count);
            con2.Open();
            //con2.Open();
            SqliteCommand dbcmd2 = new SqliteCommand(con2);

            // add the count
            string sqlQuery3 = "insert into count1 values(@Userid, @count, @attempt)";
            dbcmd2.CommandText = sqlQuery3;
            dbcmd2.Prepare();
            
            dbcmd2.Parameters.AddWithValue("@userid", 11);
            dbcmd2.Parameters.AddWithValue("@count", count);
            dbcmd2.Parameters.AddWithValue("@attempt", attempt);
            dbcmd2.ExecuteNonQuery();
            //con2.Close();

            // add the speed
            string sqlQuery_speed = "insert into avg_speed values(@Userid, @avgspeed, @attempt)";

            dbcmd2.CommandText = sqlQuery_speed;
            dbcmd2.Prepare();

            dbcmd2.Parameters.AddWithValue("@userid", 11);
            dbcmd2.Parameters.AddWithValue("@avgspeed", avgspeed);
            dbcmd2.Parameters.AddWithValue("@attempt", attempt);
            dbcmd2.ExecuteNonQuery();
            con2.Close();

           

        }

        

        
    }

	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			// Make the other game object (the pick up) inactive, to make it disappear
			other.gameObject.SetActive (false);

			// Add one to the score variable 'count'
			count = count + 1;

			// Run the 'SetCountText()' function (see below)
			SetCountText ();
		}
	}

	// Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
	void SetCountText()
	{
        // Update the text field of our 'countText' variable
        countText.text = "Count: " + count.ToString ();
        Debug.Log("learn count1" + count);
        //countText.text = "Tutorial: " + learn2[count];
        TutorialText.text = "Tutorial: " + learn2[count];

        // Check if our 'count' is equal to or exceeded 12
        if (count >= 12) 
		{
            winText.text = "You Win!";
            //Set value
            
            
            
            if(Window_Graph.flag){
                float avgSpeed2 = average_speed/count_speed;
                Debug.Log("AVGSPEED2 "+avgSpeed2);
                //Debug.Log("incrementing ...countSession ");
                Window_Graph.hitCount = count;
                Window_Graph.countSession++;
                Window_Graph.flag = false;
                
                Avg_Speed_Graph.avgSpeed = Mathf.Abs(avgSpeed2);
                Avg_Speed_Graph.countSession++;
                Avg_Speed_Graph.flag = false;
            }
            
            
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("aftergame");

            // Set the text value of our 'winText'
            string con_count = "URI=file:" + Application.dataPath + "/plugins/ballgame.db"; // path to database
            SqliteConnection con2 = new SqliteConnection(con_count);
            con2.Open();
            SqliteCommand dbcmd2 = new SqliteCommand(con2);

            // add the count
            string sqlQuery_speed = "insert into count1 values(@Userid, @count, @attempt)";
            dbcmd2.CommandText = sqlQuery_speed;
            dbcmd2.Prepare();

            dbcmd2.Parameters.AddWithValue("@userid", 11);
            dbcmd2.Parameters.AddWithValue("@count", count);
            dbcmd2.Parameters.AddWithValue("@attempt", attempt);
            dbcmd2.ExecuteNonQuery();
            //con2.Close();

            // add the speed
            string sqlQuery3 = "insert into avg_speed values(@Userid, @avgspeed, @attempt)";

            dbcmd2.CommandText = sqlQuery3;
            dbcmd2.Prepare();

            dbcmd2.Parameters.AddWithValue("@userid", 11);
            dbcmd2.Parameters.AddWithValue("@avgspeed", avgspeed);
            dbcmd2.Parameters.AddWithValue("@attempt", attempt);
            dbcmd2.ExecuteNonQuery();
            con2.Close();
            
            //yield return new WaitForSeconds(3);
  		}
    }
}