using UnityEngine;

public class UnpackingRing : MonoBehaviour {

    private void Awake()
    {
        Instantiate(Data.Instance.Angle45Ledge, transform.position, transform.rotation,transform.parent);
        Transform tr = Instantiate(Data.Instance.Angle45Ledge, transform.position, transform.rotation,transform.parent).transform;
        tr.Rotate(0,0,tr.localRotation.eulerAngles.z-45);
        Destroy(gameObject);
    }
}
