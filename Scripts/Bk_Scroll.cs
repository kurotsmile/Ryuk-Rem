using UnityEngine;

public class Bk_Scroll : MonoBehaviour
{
    public GameObject[] barrier_left;
    public GameObject[] barrier_right;
    public GameObject[] apple;

    public void change_layout()
    {
        for(int i = 0; i < this.barrier_left.Length; i++)
        {
            if (Random.Range(0, 2) == 0)
                barrier_left[i].transform.localPosition = new Vector3(-5.11f, barrier_left[i].transform.localPosition.y, barrier_left[i].transform.localPosition.z);
            else
                barrier_left[i].transform.localPosition = new Vector3(-2.65f, barrier_left[i].transform.localPosition.y, barrier_left[i].transform.localPosition.z);
        }

        for (int i = 0; i < this.barrier_right.Length; i++)
        {
            if (Random.Range(0, 2) == 0)
                barrier_right[i].transform.localPosition = new Vector3(2.96f, barrier_right[i].transform.localPosition.y, barrier_right[i].transform.localPosition.z);
            else
                barrier_right[i].transform.localPosition = new Vector3(5.51f, barrier_right[i].transform.localPosition.y, barrier_right[i].transform.localPosition.z);
        }

        for (int i = 0; i < this.apple.Length; i++) this.apple[i].SetActive(true);
    }
}
