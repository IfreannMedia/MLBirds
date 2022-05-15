using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
	int DNALength = 5;
	public DNA dna;
    public GameObject eyes;
   public bool seeDownWall = false; 
   public bool seeUpWall = false; 
   public bool seeBottom = false;
   public bool seeTop = false;  
    Vector3 startPosition;  
    public float timeAlive = 0;
    public float distanceTravelled = 0;  
    public int crash = 0;
    bool alive = true;  
    Rigidbody2D rb;    
    
	public void Init()
	{
		//initialise DNA
        //0 forward
        //1 upwall
        //2 downwall
        //3 normal upward
        dna = new DNA(DNALength,200);
        this.transform.Translate(Random.Range(-1.5f,1.5f),Random.Range(-1.5f,1.5f),0);
        startPosition = this.transform.position;
        rb = this.GetComponent<Rigidbody2D>();
	}

    void OnCollisionEnter2D(Collision2D col)
    {

    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "dead" ||
            col.gameObject.tag == "top" ||
            col.gameObject.tag == "bottom" ||
            col.gameObject.tag == "upwall" ||
            col.gameObject.tag == "downwall")
        {
            crash++;
        }
    }


    void Update()
    {
        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 1.0f, seeUpWall || seeDownWall ? Color.red : Color.green);
        Debug.DrawRay(eyes.transform.position, eyes.transform.up * 1.0f, seeTop ? Color.red : Color.green);
        Debug.DrawRay(eyes.transform.position, -eyes.transform.up * 1.0f, seeBottom ? Color.red : Color.green);

        if (!alive) return;

        seeUpWall = false;
        seeDownWall = false;
        seeTop = false;
        seeBottom = false;
        LayerMask mask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.forward, 1.0f, mask);
        if (hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "upwall")
            {
                seeUpWall = true;
            }
            else if(hit.collider.gameObject.tag == "downwall")
            {
                seeDownWall = true;
            }
        }
		hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.up, 1.0f, mask);
		if (hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "top")
            {
                seeTop = true;
            }
        }
        hit = Physics2D.Raycast(eyes.transform.position, -eyes.transform.up, 1.0f, mask);
		if (hit.collider != null)
        {    
            if(hit.collider.gameObject.tag == "bottom")
            {
                seeBottom = true;
            }
        }

        timeAlive = PopulationManager.elapsed;
    }


    void FixedUpdate()
    {
        if(!alive) return;
        
        // read DNA
        float h = 0;
        float v = 1.0f; //dna.GetGene(0);

        if(seeUpWall)
        { 
            h = dna.GetGene(0);
        }
        else if(seeDownWall)
        {
        	h = dna.GetGene(1);
        }
        else if(seeTop)
        {
        	h = dna.GetGene(2);
        }        
        else if(seeBottom)
        {
        	h = dna.GetGene(3);
        }
        else
        {
        	h = dna.GetGene(4);
        }

        rb.AddForce(this.transform.right * v);
        rb.AddForce(this.transform.up * h * 0.1f);
        distanceTravelled = Vector3.Distance(startPosition,this.transform.position);
    }
}

