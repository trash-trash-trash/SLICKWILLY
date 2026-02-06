using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
	[Header("Rope Segments")]
	[Tooltip("Array of rigidbodies that make up the rope segments")]
	public Rigidbody[] ropeSegments;

	[Header("Line Renderer Settings")]
	[Tooltip("Width of the rope line at the start")]
	public float startWidth = 0.1f;

	[Tooltip("Width of the rope line at the end")]
	public float endWidth = 0.1f;

	[Tooltip("Material for the rope (optional)")]
	public Material ropeMaterial;

	private LineRenderer lineRenderer;

	public bool turnOffRenderers = true;

	void Start()
	{
		// Get or add LineRenderer component
		lineRenderer = GetComponent<LineRenderer>();
		if (lineRenderer == null)
		{
			lineRenderer = gameObject.AddComponent<LineRenderer>();
		}

		// Configure LineRenderer
		SetupLineRenderer();
	}

	void SetupLineRenderer()
	{
		if (ropeSegments == null || ropeSegments.Length == 0)
		{
			Debug.LogWarning("No rope segments assigned!");
			return;
		}

		// Set the number of positions to match the number of segments
		lineRenderer.positionCount = ropeSegments.Length;

		// Set width
		lineRenderer.startWidth = startWidth;
		lineRenderer.endWidth   = endWidth;

		// Set material if provided
		if (ropeMaterial != null)
		{
			lineRenderer.material = ropeMaterial;
		}

		// Optional: disable world space if you want local positioning
		lineRenderer.useWorldSpace = true;

		// Optional: improve visual quality
		lineRenderer.numCapVertices    = 5;
		lineRenderer.numCornerVertices = 5;

		if (turnOffRenderers)
			foreach (Rigidbody ropeSegment in ropeSegments)
			{
				Renderer rend = ropeSegment.GetComponent<Renderer>();
				if (rend != null)
					rend.enabled = false;
			}
	}

	void LateUpdate()
	{
		if (ropeSegments == null || ropeSegments.Length == 0)
			return;

		// Update line renderer positions to follow the rigidbodies
		for (int i = 0; i < ropeSegments.Length; i++)
		{
			if (ropeSegments[i] != null)
			{
				lineRenderer.SetPosition(i, ropeSegments[i].transform.position);
			}
		}
	}

	// Optional: Helper method to automatically find rope segments in children
	[ContextMenu("Auto-Assign Child Rigidbodies")]
	void AutoAssignChildRigidbodies()
	{
		ropeSegments = GetComponentsInChildren<Rigidbody>();
		Debug.Log($"Auto-assigned {ropeSegments.Length} rope segments");
	}
}