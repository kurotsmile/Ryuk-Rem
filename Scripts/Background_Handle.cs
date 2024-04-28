using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background_Handle : MonoBehaviour
{
    private bool is_play;
    public GameObject[] barrier_vertical_right;
    public GameObject[] barrier_vertical_left;
    public GameObject[] obj_bk_scroll;
    public Text txt_kilometer;
    public Text txt_apple;
    private float speed_scroll = 1f;
    private float kilometer = 0f;
    private int apple = 0;

    public void load_bk()
    {
        this.is_play = false;
    }

    public void play()
    {
        this.apple = 0;
        this.txt_apple.text = "0";
        this.speed_scroll = 1f;
        this.is_play = true;
        this.txt_kilometer.text = "0";
        this.kilometer = 0f;
        for (int i = 0; i < this.obj_bk_scroll.Length; i++) this.obj_bk_scroll[i].GetComponent<Bk_Scroll>().change_layout();

        this.obj_bk_scroll[0].transform.position = new Vector3(0f, 10.25f,0f);
        this.obj_bk_scroll[1].transform.position = new Vector3(0f, 0f, 0f);
        this.obj_bk_scroll[2].transform.position = new Vector3(0f, -10.25f, 0f);
    }

    public void stop()
    {
        this.is_play = false;
    }

    void Start()
    {
        Vector2 worldBoundary = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        float w = worldBoundary.x;
        for(int i=0;i<this.barrier_vertical_left.Length;i++)
        {
            this.barrier_vertical_left[i].transform.position = new Vector3(-w, this.barrier_vertical_left[i].transform.position.y, 0);
            this.barrier_vertical_right[i].transform.position = new Vector3(w, this.barrier_vertical_right[i].transform.position.y, 0);
        }
    }

    void Update()
    {
        if (this.is_play)
        {
            this.kilometer += (0.001f+(this.speed_scroll/2)) * Time.deltaTime;
            this.txt_kilometer.text = this.kilometer.ToString("f2")+" km";
            for (int i = 0; i < this.obj_bk_scroll.Length; i++)
            {
                this.obj_bk_scroll[i].transform.Translate(Vector3.down * (this.speed_scroll * Time.deltaTime));
                if (this.obj_bk_scroll[i].transform.position.y <= -10.16f)
                {
                    this.obj_bk_scroll[i].transform.position = new Vector3(0f, 20.44f);
                    this.obj_bk_scroll[i].GetComponent<Bk_Scroll>().change_layout();
                    this.speed_scroll += 0.3f;
                }
            }
        }
    }

    public void add_apple()
    {
        this.apple++;
        this.txt_apple.text = this.apple.ToString();
    }

    public int get_apple()
    {
        return this.apple;
    }

    public float get_kilometer()
    {
        return this.kilometer;
    }
}
