using UnityEngine;
using System.Collections;

public class Effects_Circle : MonoBehaviour 
{
	public Color startColor;
	private Color currentColor;
	private LineRenderer LR;

	[SerializeField]
	private bool dieOnEnd;
	private bool expanded, start, circleRunning;

	[SerializeField]
	private int segments= 36;
    private int vertices;

	[SerializeField]
   	private float startTime, radius, expandRate, transparencyRate, finalRadius;

	[SerializeField]
	private string type;
	private string id;

	public string ID
	{
		get
		{ return id; }
		set
		{ id = value; }
	}

    void Start()
    {
        Invoke("CallStart", this.startTime);
    }

    void CallStart()
	{
        LR = gameObject.GetComponent<LineRenderer>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        LR.sortingLayerID = spriteRenderer.sortingLayerID;
        LR.sortingOrder = spriteRenderer.sortingOrder;

        currentColor = startColor;
        LR.SetColors(currentColor, currentColor);
        LR.SetVertexCount(segments + 1);
        vertices = segments + 1;
        LR.useWorldSpace = false;
        CreatePoints(radius, radius);
        StartCircle(radius, expandRate, transparencyRate);
        this.start = true;
    }

    // Update is called once per frame
    void Update ()
    {
        //GetComponent<CircleCollider2D>().radius = radius;
        if(start)
        {
	        if (circleRunning)
	        {
	        	if((this.radius < this.finalRadius && this.finalRadius != 0) || this.finalRadius == 0) 
		        {
		            currentColor.a -= Time.deltaTime * transparencyRate;
					radius += Time.deltaTime * expandRate;
	            	CreatePoints(radius);
	            }
	            else if(this.finalRadius != 0)
	            {
					this.radius = this.finalRadius;
	            	CreatePoints(radius);
	            }
	        }
	        else
	        {
	            currentColor.a = 0f;
	        }

	        if (currentColor.a <= 0)
	        {
	            if (dieOnEnd)
	            {
	                Destroy(gameObject);
	                Destroy(this);
	            }
	            circleRunning = false;
	        }

	        LR.SetColors(currentColor, currentColor);
        }
    }

    void StartCircle(float rad, float expand, float transp)
    {
        currentColor = startColor;
        LR.SetColors(currentColor, currentColor);
        radius = rad;
        expandRate = expand;
        transparencyRate = transp;
        circleRunning = true;
    }
		
    void CreatePoints(float radius)
    {
        CreatePoints(radius, radius);
    }

    void CreatePoints(float x, float y)
    {
        if (segments != vertices)
        {
            LR.SetVertexCount(segments + 1);
            vertices = segments + 1;
        }

        float z = 1;
        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            LR.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }
}
