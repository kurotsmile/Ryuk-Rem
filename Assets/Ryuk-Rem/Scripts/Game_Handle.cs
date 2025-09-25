using Carrot;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Handle : MonoBehaviour
{
    [Header("Main Object")]
    public Carrot.Carrot carrot;
    public GameObject obj_main;
    public Animator anim_cam;
    public Background_Handle bk;
    public Death death_left;
    public Death death_right;
    public GameObject[] effect_prefab;
    private int discipline_score = 0;
    private float longest_distance=0;

    [Header("UI Object")]
    public GameObject panel_main;
    public GameObject panel_play;
    public GameObject panel_gameover;

    [Header("Sound")]
    public AudioClip audio_clip_click;
    public AudioSource[] sound;

    [Header("GameOver")]
    public Text txt_gameover_apple_red;
    public Text txt_gameover_apple_green;
    public Text txt_gameover_discipline_score;
    public Text txt_gameover_your_score;
    public Text txt_gameover_longest_distance;
    public Text txt_gameover_traveled_distance;

    [Header("Gamepad")]
    public List<GameObject> list_btn_main;
    public List<GameObject> list_btn_gameover;

    void Start()
    {
        this.carrot.Load_Carrot(this.check_exit_app);
        this.carrot.change_sound_click(this.audio_clip_click);
        this.carrot.act_after_close_all_box = act_after_close_all_box;

        Carrot_Gamepad death_player=this.carrot.game.create_gamepad("death_player");
        death_player.set_gamepad_keydown_left(this.keydown_gamepad_left);
        death_player.set_gamepad_keydown_right(this.keydown_gamepad_right);
        death_player.set_gamepad_keydown_up(this.keydown_gamepad_up);
        death_player.set_gamepad_keydown_down(this.keydown_gamepad_down);
        death_player.set_gamepad_keydown_select(this.keydown_gamepad_select);
        death_player.set_gamepad_keydown_start(this.keydown_gamepad_start);

        this.discipline_score = PlayerPrefs.GetInt("discipline_score", 0);
        this.longest_distance = PlayerPrefs.GetFloat("longest_distance", 0f);

        this.obj_main.SetActive(true);
        this.panel_main.SetActive(true);
        this.panel_play.SetActive(false);
        this.panel_gameover.SetActive(false);
        this.bk.load_bk();
        this.death_left.load(Death_type.left,this);
        this.death_right.load(Death_type.right,this);
        this.anim_cam.enabled = false;
        this.carrot.game.set_list_button_gamepad_console(this.list_btn_main);

        this.carrot.game.load_bk_music(this.sound[0]);
    }

    private void act_after_close_all_box()
    {
        this.carrot.game.set_list_button_gamepad_console(this.list_btn_main);
    }

    private void check_exit_app()
    {
        if (this.panel_play.activeInHierarchy)
        {
            this.btn_back_menu();
            this.carrot.set_no_check_exit_app();
        }else if (this.panel_gameover.activeInHierarchy)
        {
            this.btn_back_menu();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void btn_game_play()
    {
        this.carrot.game.clear_button_gamepad_console();
        this.death_left.reset();
        this.death_right.reset();
        this.death_left.play();
        this.death_right.play();
        this.obj_main.SetActive(false);
        this.carrot.play_sound_click();
        this.panel_main.SetActive(false);
        this.panel_play.SetActive(true);
        this.panel_gameover.SetActive(false);
        this.bk.play();
    }

    public void btn_back_menu()
    {
        this.death_left.reset();
        this.death_right.reset();
        this.obj_main.SetActive(true);
        this.carrot.play_sound_click();
        this.panel_main.SetActive(true);
        this.panel_play.SetActive(false);
        this.panel_gameover.SetActive(false);
        this.bk.stop();
        this.carrot.game.set_list_button_gamepad_console(this.list_btn_main);
        this.carrot.ads.show_ads_Interstitial();
    }

    public void show_game_over()
    {
        if (this.bk.get_apple() > this.discipline_score)
        {
            this.discipline_score = this.bk.get_apple();
            PlayerPrefs.SetInt("discipline_score", this.discipline_score);
        }

        if (this.bk.get_kilometer() > this.longest_distance)
        {
            this.longest_distance = this.bk.get_kilometer();
            PlayerPrefs.SetFloat("longest_distance", this.longest_distance);
        }

        this.txt_gameover_apple_red.text = this.death_left.get_apple().ToString();
        this.txt_gameover_apple_green.text = this.death_right.get_apple().ToString();
        this.txt_gameover_your_score.text = this.bk.get_apple().ToString();
        this.txt_gameover_discipline_score.text = this.discipline_score.ToString();
        this.txt_gameover_longest_distance.text = this.longest_distance.ToString("f2") + " Km";
        this.txt_gameover_traveled_distance.text = this.bk.get_kilometer().ToString("f2")+" Km";
        this.carrot.game.update_scores_player(this.bk.get_apple());
        this.anim_cam.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        this.anim_cam.enabled = false;
        this.death_left.stop();
        this.death_right.stop();
        this.play_sound(1);
        this.panel_gameover.SetActive(true);
        this.panel_main.SetActive(false);
        this.panel_play.SetActive(false);
        this.carrot.game.set_list_button_gamepad_console(this.list_btn_gameover);
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.play_vibrate();
    }

    public void btn_top_player()
    {
        this.carrot.game.Show_List_Top_player();
    }

    public void btn_show_setting()
    {
        Carrot_Box box_setting=this.carrot.Create_Setting();
        box_setting.update_gamepad_cosonle_control();
        box_setting.set_act_before_closing(this.act_after_close_setting);
        this.carrot.ads.show_ads_Interstitial();
    }

    public void act_after_close_setting(List<string> item_change)
    {
        foreach(string s in item_change)
        {
            if (s == "list_bk_music") this.carrot.game.load_bk_music(this.sound[0]);
        }

        if (this.carrot.get_status_sound())
            this.sound[0].Play();
        else
            this.sound[0].Stop();
    }

    public void btn_user_carrot()
    {
        this.carrot.user.show_login();
    }

    public void death_left_evade()
    {
        this.death_left.evade();
    }

    public void death_right_evade()
    {
        this.death_right.evade();
    }

    private void keydown_gamepad_down()
    {
        this.carrot.game.gamepad_keydown_down_console();
    }

    private void keydown_gamepad_up()
    {
        this.carrot.game.gamepad_keydown_up_console();
    }

    private void keydown_gamepad_left()
    {
        this.death_left_evade();
        this.carrot.game.gamepad_keydown_up_console();
    }

    private void keydown_gamepad_right()
    {
        this.death_right_evade();
        this.carrot.game.gamepad_keydown_down_console();
    }

    private void keydown_gamepad_start()
    {
        this.carrot.game.gamepad_keydown_enter_console();
    }

    private void keydown_gamepad_select()
    {
        this.carrot.game.gamepad_keydown_enter_console();
    }

    public void play_sound(int index_sound)
    {
        if (this.carrot.get_status_sound()) this.sound[index_sound].Play();
    }

    public void create_effect(Vector3 pos,int index_effect=0)
    {
        GameObject obj_effect =Instantiate(this.effect_prefab[index_effect]);
        obj_effect.transform.SetParent(this.transform);
        obj_effect.transform.position = pos;
        Destroy(obj_effect, 2f);
    }

    public void btn_rate()
    {
        this.carrot.show_rate();
    }
}
