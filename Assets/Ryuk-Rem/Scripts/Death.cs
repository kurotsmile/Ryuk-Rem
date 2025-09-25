using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Death_type { left, right }
public class Death : MonoBehaviour
{
    Death_type type;
    Death_type pos_type;
    private float pos_x_val = 2.8f;
    public Image img_evade;
    public GameObject obj_Death;
    private Game_Handle game;
    private int apple;
    private bool is_play;
    public void load(Death_type death_type,Game_Handle g)
    {
        this.obj_Death.SetActive(true);
        this.game = g;
        this.type = death_type;
        this.is_play = false;
        if(this.type==Death_type.left)
            this.pos_type = Death_type.right;
        else
            this.pos_type = Death_type.left;

        this.check_icon_evade();
    }

    public void play()
    {
        this.is_play = true;
    }

    public void stop()
    {
        this.is_play = false;
    }

    public void evade()
    {
        if (this.is_play)
        {
            this.game.play_sound(4);
            if (this.pos_type == Death_type.right)
            {
                this.pos_type = Death_type.left;
                this.transform.Translate(Vector3.left * this.pos_x_val);
            }
            else
            {
                this.pos_type = Death_type.right;
                this.transform.Translate(Vector3.right * this.pos_x_val);
            }
            this.check_icon_evade();
        }
    }

    private void check_icon_evade()
    {
        if (this.pos_type == Death_type.right)
        {
            this.img_evade.transform.localScale = new Vector3(1f, 1f);
        }
        else
        {
            this.img_evade.transform.localScale = new Vector3(-1f, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "apple")
        {
            this.apple++;
            if (this.type == Death_type.left)
                this.game.create_effect(this.transform.position, 1);
            else
                this.game.create_effect(this.transform.position, 2);
            this.game.bk.add_apple();
            collision.gameObject.SetActive(false);
            this.game.play_sound(3);
        }
        else
        {
            this.die();
        }
    }

    private void die()
    {
        this.game.carrot.play_vibrate();
        this.is_play = false;
        this.game.anim_cam.enabled = true;
        if(this.game.anim_cam.enabled) this.game.anim_cam.Play("game");
        this.obj_Death.SetActive(false);
        this.game.play_sound(2);
        this.game.create_effect(this.transform.position);
        this.game.panel_play.SetActive(false);
        this.game.bk.stop();
        this.game.carrot.delay_function(1f, this.show_game_over);
    }

    public void show_game_over()
    {
        this.game.show_game_over();
    }

    public void reset()
    {
        if (this.type == Death_type.left)
        {
            this.pos_type = Death_type.right;
            this.transform.position = new Vector3(-2.62f, -2.71f, -2f);
        }
        else
        {
            this.pos_type = Death_type.left;
            this.transform.position = new Vector3(2.81f, -2.71f, -2f);
        } 
        this.apple = 0;
        this.obj_Death.SetActive(true);
    }

    public int get_apple()
    {
        return this.apple;
    }
}
