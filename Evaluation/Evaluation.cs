//風水評価クラス(多分furnitureManagementと同じように空のオブジェクトに放り込んで実装),動くかどうかのテストは行っていない

using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq; //(2018年 2月15日追加)
using UnityEngine;
using UnityEngine.UI;

public partial class Evaluation : MonoBehaviour
{
    private const int limit_elements_ = 500;
    private const int limit_yin_ = -300;
    private const int limit_yang_ = 300;

    private const int limit_bed_ = 2;
    private const int limit_cabinet_ = 5;
    private const int limit_carpet_ = 3;
    private const int limit_desk_ = 3;
    private const int limit_foliage_ = 5;
    private const int limit_lamp_ = 4;
    private const int limit_sofa_ = 4;
    private const int limit_table_ = 2;
    private const int limit_electronics_ = 5;
    private const int limit_furniture_ = 13;
    private const int limit_furniture_few_ = 3;

    private const int limit_red_color_ = 3;
    private const int limit_high_form_ = 5;

    public enum Room { Entrance, Living, Kitchen, Workroom, Bedroom, Bathroom, Toilet };
    private Room room_role_; //部屋の種類

    public enum Direction { North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest };
    private Direction room_direction_; //部屋の方角

    public enum CommentIdentifier
    {
        //気と陰陽に関係するもの 
        OverYin, OverYang, //陰陽の強すぎ
        NorthWeak, NorthEastWeak, NorthEastMinus, EastWeak, SouthEastWeak, SouthWeak, SouthWestWeak, WestWeak, NorthWestWeak, //方角基本性能
        NorthWeakOther, NorthEastWeakOther, EastWeakOther, SouthEastWeakOther, SouthWeakOther, SouthWestWeakOther, WestWeakOther, NorthWestWeakOther, //方角基本性能(その方向の気以外)
        SplitNorthWeak, SplitNorthEastWeak, SplitNorthEastMinus, SplitEastWeak, SplitSouthEastWeak, SplitSouthWeak, SplitSouthWestWeak, SplitWestWeak, SplitNorthWestWeak, //方角基本性能(小方位)
        SouthPurification, NorthWestVain,//方角その他
        WoodOver, FireOver, EarthOver, MetalOver, WaterOver, //気が強すぎ

        //相生効果に関するコメント
        WoodSoshoNorth, FireSoshoNorth, EarthSoshoNorth, MetalSoshoNorth, WaterSoshoNorth, //相生(ゲーム中のみ)
        WoodSoshoNorthEast, FireSoshoNorthEast, EarthSoshoNorthEast, MetalSoshoNorthEast, WaterSoshoNorthEast, //相生(ゲーム中のみ)
        WoodSoshoEast, FireSoshoEast, EarthSoshoEast, MetalSoshoEast, WaterSoshoEast, //相生(ゲーム中のみ)
        WoodSoshoSouthEast, FireSoshoSouthEast, EarthSoshoSouthEast, MetalSoshoSouthEast, WaterSoshoSouthEast, //相生(ゲーム中のみ)
        WoodSoshoSouth, FireSoshoSouth, EarthSoshoSouth, MetalSoshoSouth, WaterSoshoSouth, //相生(ゲーム中のみ)
        WoodSoshoSouthWest, FireSoshoSouthWest, EarthSoshoSouthWest, MetalSoshoSouthWest, WaterSoshoSouthWest, //相生(ゲーム中のみ)
        WoodSoshoWest, FireSoshoWest, EarthSoshoWest, MetalSoshoWest, WaterSoshoWest, //相生(ゲーム中のみ)
        WoodSoshoNorthWest, FireSoshoNorthWest, EarthSoshoNorthWest, MetalSoshoNorthWest, WaterSoshoNorthWest, //相生(ゲーム中のみ)
        WoodSosho, FireSosho, EarthSosho, MetalSosho, WaterSosho, //相生(ゲーム終了時)

        //相克効果に関するコメント
        WoodSokokuNorth, FireSokokuNorth, EarthSokokuNorth, MetalSokokuNorth, WaterSokokuNorth,  //相克(ゲーム中のみ)
        WoodSokokuNorthEast, FireSokokuNorthEast, EarthSokokuNorthEast, MetalSokokuNorthEast, WaterSokokuNorthEast,  //相克(ゲーム中のみ)
        WoodSokokuEast, FireSokokuEast, EarthSokokuEast, MetalSokokuEast, WaterSokokuEast,  //相克(ゲーム中のみ)
        WoodSokokuSouthEast, FireSokokuSouthEast, EarthSokokuSouthEast, MetalSokokuSouthEast, WaterSokokuSouthEast,  //相克(ゲーム中のみ)
        WoodSokokuSouth, FireSokokuSouth, EarthSokokuSouth, MetalSokokuSouth, WaterSokokuSouth,  //相克(ゲーム中のみ)
        WoodSokokuSouthWest, FireSokokuSouthWest, EarthSokokuSouthWest, MetalSokokuSouthWest, WaterSokokuSouthWest,  //相克(ゲーム中のみ)
        WoodSokokuWest, FireSokokuWest, EarthSokokuWest, MetalSokokuWest, WaterSokokuWest,  //相克(ゲーム中のみ)
        WoodSokokuNorthWest, FireSokokuNorthWest, EarthSokokuNorthWest, MetalSokokuNorthWest, WaterSokokuNorthWest,  //相克(ゲーム中のみ)
        WoodSokoku, FireSokoku, EarthSokoku, MetalSokoku, WaterSokoku,  //相克(ゲーム終了時)

        //気を上げましょう
        WoodWeakNorth, FireWeakNorth, EarthWeakNorth, MetalWeakNorth, WaterWeakNorth,  //気が低い(ゲーム中のみ)
        WoodWeakNorthEast, FireWeakNorthEast, EarthWeakNorthEast, MetalWeakNorthEast, WaterWeakNorthEast,  //気が低い(ゲーム中のみ)
        WoodWeakEast, FireWeakEast, EarthWeakEast, MetalWeakEast, WaterWeakEast,  //気が低い(ゲーム中のみ)
        WoodWeakSouthEast, FireWeakSouthEast, EarthWeakSouthEast, MetalWeakSouthEast, WaterWeakSouthEast,  //気が低い(ゲーム中のみ)
        WoodWeakSouth, FireWeakSouth, EarthWeakSouth, MetalWeakSouth, WaterWeakSouth,  //気が低い(ゲーム中のみ)
        WoodWeakSouthWest, FireWeakSouthWest, EarthWeakSouthWest, MetalWeakSouthWest, WaterWeakSouthWest,  //気が低い(ゲーム中のみ)
        WoodWeakWest, FireWeakWest, EarthWeakWest, MetalWeakWest, WaterWeakWest,  //気が低い(ゲーム中のみ)
        WoodWeakNorthWest, FireWeakNorthWest, EarthWeakNorthWest, MetalWeakNorthWest, WaterWeakNorthWest,  //気が低い(ゲーム中のみ)
        WoodWeak, FireWeak, EarthWeak, MetalWeak, WaterWeak,  //気を上げましょう(ゲーム終了時)

        //ボーナス点に関係するもの
        //部屋関連
        BedroomMulti, //寝室関連(ゲーム終了専用)
        LivingMulti, //リビング関連(ゲーム終了専用)
        WorkroomMulti, //仕事部屋関連(ゲーム終了専用)


        //方角関連
        NorthCold,//北の運勢
        //北東の運勢
        //東の運勢
        //南東の運勢
        //南の運勢
        //南西の運勢
        //西の運勢
        //北西の運勢


        //家具関連
        BedLiving, BedWorkroom, BedNoBedroom, BedNatural, BedConnected, BedGapWall, BedToDoor, BedNearWindow, BedOver, //ベッド関連
        BedSouthToNorth, BedSouthToEast, BedWestToNorth, BedWestToEast, BedNorthToEast, BedEastToNorth, //ベッドの向き(ゲーム中専用)
        BedSouthDirection, BedWestDirection, //ベッドの向き(ゲーム終了専用) 
        CabinetOver, //タンス関連
        CarpetOver,//カーペット関連
        DeskBedroom, DeskNoWorkRoom, DeskFrontWindow, DeskSeatNearWall, DeskOver, //机関連
        DeskNorthEastToSouth, DeskNorthEastToWest, DeskSouthToNorthEast, DeskSouthToWest, DeskWestToNorthEast, DeskWestToSouth, //机向き(タイプ1専用)
        DeskNoNorthEast, DeskNoSouth, DeskNoWest, //机向き(タイプ2専用)
        FoliagePurification, FoliagePlantOver,//観葉植物関連
        LampOver, LampNo,//天井ランプ関連 
                         //机ランプ
        SofaNoLiving, SofaSplitWest, SofaToDoor, SofaOver,//ソファー関連
        TableNoLiving, TableOver,//テーブル関連 
        ElectronicsSouth, ElectronicsNoEast, TVNoToWest, ElectronicsToWindowDoor, ElectronicsToBed, ElectronicsOver,//家電関連 ( 4個まで )                                                                                                     
        FurnitureFew, FurnitureOver, //家具多すぎと少なすぎ


        //色関連
        WhiteColorResetYinYang, WhiteColorPurification, //白
        BlackStrengthening, BlackNoStrengthening, BlackNoGreemWarm, BlackInteger, //黒
        GrayNorthWest, GraySplitNorthWest, //灰色
                                           //濃い灰色
        RedOne, RedOver,//赤
        PinkColorOne, PinkColorNoOrange, PinkBed, PinkNorth, PinkSplitNorth, //ピンク
        BlueColorOne, BlueInteger, BlueNoGreenWarm, //青
        LightBlueColorNoOrange, //水色
        OrangeColorNoPink, OrangeColorNoLightBlue, OrangeSouthEast, OrangeSplitSouthEast, //オレンジ
        YellowBrownOcherOne, //黄色
        //緑
        //黄緑
        BeigeCreamNorthWest, BeigeCreamSplitNorthWest, //ベージュ
                                                       //クリーム色
                                                       //茶
                                                       //黄土色
        GoldOne, GoldBad, //金
        SilverInteger, //銀
                       //紫


        //材質関連
        //人工観葉植物
        WoodNaturalCottonBedroom,  //木製
                                   //紙
                                   //革
                                   //天然繊維
        CemicalPlusticOne, //化学繊維
                           //綿
                           //プラスチック
                           //陶磁器
                           //大理石
                           //金属
                           //鉱物
                           //ガラス
                           //水


        //模様関連
        //ストライプ
        //リーフパターン
        //花柄
        //星柄
        //ダイヤ柄
        //アニマル柄
        //ジグザグ
        //奇抜
        //ボーダー
        //チェック(市松)
        //タイル柄
        //ドット,
        //丸柄,
        //アーチ
        //フルーツ
        //光沢
        //ウェーブストライプ
        //不規則パターン
        //雲柄


        //形状関連
        HighFormNorthEast, HighFormSplitNorthEast, HighOver,//背が高い
        LowFormSouthWest, LowFormSplitSouthWest, //背が低い
                                                 //縦長
                                                 //横長
        SquareOne, SquareBad,                 //正方形
        RectangleMulti,              //長方形
        RoundOne, RoundBad,                  //円形
        EllipseMulti,                //楕円形
                                     //三角形
        SharpBad,                  //尖っている
                                     //奇抜な形状


        LuxuryNorthWest, LuxurySplitNorthWest, LuxuryZeroNorthWest,//高級そう
        SoundEast, SoundSouthEast, SoundSplitEastSouthEast, //音が出る
                                                            //(いい)におい
                                                            //発光
                                                            //硬い
                                                            //やわらかい
        WarmInteger, //温かみ
        ColdNorth, ColdSplitNorth, //冷たさ
                                   //花関連
                                   //風関連
        WesternWest, WesternSplitWest, //西洋風
        //奇抜
        //乱雑
    };

    private enum AdviceType
    {
        Element, ElementGame, ElementEnd, Bonus, BonusGame, BonusEnd
    }
    private class CommentFlag
    {
        public CommentIdentifier comment_identifier_; //コメント識別子
        public int work_weight_;
        public int popular_weight_;
        public int health_weight_;
        public int economic_weight_;
        public int love_weight_;
        public AdviceType advice_type_; //五行陰陽に関係するもの = 0，ボーナス点ペナルティ点に関係するもの = 1;
        public int comment_weight_ = 0;
        public CommentFlag(CommentIdentifier comment_identifier, int work_weight, int popular_weight, int health_weight, int economic_weight, int love_weight, AdviceType advice_type)
        {
            comment_identifier_ = comment_identifier;
            work_weight_ = work_weight;
            popular_weight_ = popular_weight;
            health_weight_ = health_weight;
            economic_weight_ = economic_weight;
            love_weight_ = love_weight;
            advice_type_ = advice_type;
        }
    }

    private List<CommentFlag> comment_flag_ = new List<CommentFlag>();
    private List<string> comment_ = new List<string>(); //コメント ( コメントフラグに応じていくつかのコメントを出力 )


    public enum EnergyIdentifier
    {
        LampNo, //ランプ関連
    }
    private class EnergyFlag
    {

    }


    public enum CharacteristicIdentifier
    {
        //家具のタイプ
        BedFurniture, DeskFurniture, TableFurniture, SofaFurniture, FoliagePlantFurniture, CarpetFurniture, CurtainFurniture,
        ConsumerElectronicsFurniture, DresserFurniture, CeilLampFurniture, DeskLampFurniture, WindowFurniture, DoorFurniture, CabinetFurniture,

        //カラー
        WhiteColor, BlackColor, GrayColor, DarkGrayColor, RedColor, PinkColor, BlueColor, LightBlueColor, OrangeColor,
        YellowColor, GreenColor, LightGreenColor, BeigeColor, CreamColor, BrownColor, OcherColor, GoldColor, SilverColor, PurpleColor,

        //材質
        ArtificialFoliageMaterial, WoodenMaterial, PaperMaterial, LeatherMaterial, NaturalFibreMaterial, ChemicalFibreMaterial,
        CottonMaterial, PlasticMaterial, CeramicMaterial, MarbleMaterial, MetalMaterial, MineralMaterial, GlassMaterial, WaterMaterial,

        //模様
        StripePattern, LeafPattern, FlowerPattern, StarPattern, DiamondPattern, AnimalPattern, ZigzagPattern, NovelPattern, BorderPattern, CheckPattern, TilePattern,
        DotPattern, RoundPattern, ArchPattern, FruitsPattern, LusterPattern, WavePattern, IrregularityPattern, CloudPattern,

        //形状
        HighForm, LowForm, VerticalForm, OblongForm, SquareForm, RectangleForm, RoundForm, EllipseForm, TriangleForm, SharpForm, NovelForm,

        //その他の特性
        Luxury, Sound, Smell, Light, Hard, Soft, Warm, Cold, Flower, Wind, Western, Clutter
    }
    private class CharacteristicNumber
    {
        public CharacteristicIdentifier characteristic_identifier_; //特徴の種類
        public int weight_total_; //特徴のウェイト合計
        public int count_; //特徴を持っている家具の数

        public CharacteristicNumber(CharacteristicIdentifier characteristic_identifier, int weight_total, int count)
        {
            characteristic_identifier_ = characteristic_identifier;
            weight_total_ = weight_total;
        }

        public void WeightPlus(int add_weight)
        {
            weight_total_ += add_weight;
        }

        public void CountPlus(int add_count)
        {
            count_ += add_count;
        }
    }
    private List<CharacteristicNumber> characteristic_number_ = new List<CharacteristicNumber>();

    private int[] elements_ = new int[5] { 0, 0, 0, 0, 0 };  //elements_の各要素について
    private int yin_yang_ = 0; //陰陽(プラスで陽，マイナスで陰)

    private int energy_strength_ = 0; //気の強さ(max1000 min0, ここは確定するように調整)


    private int[][] split_elements_ = new int[5][];
    private int[] split_yin_yang_ = new int[8];  //部屋の中の方位ごとの陰陽

    //ここから評価用のバッファ(これらはすべて気の変化を保存する)----------------------------------------------------------------------------------------------------
    private int[][] sosho_buffer_ = new int[5][]; //相生効果によって変化した気の量
    private int[][] sokoku_buffer_ = new int[5][];  //相克効果によって変化した気の量
    //---------------------------------------------------------------------------------------------------------------------------------


    private int[] luck_ = new int[5] { 0, 0, 0, 0, 0 };  //運気(旺気と邪気を合わせた最終結果)
    private int all_luck_ = 0; //総合運
    private int[] norma_luck_ = new int[5];  //(運気の)ノルマ変数
    private int all_norma_ = 0; //総合運のノルマ
    private int[] plus_luck_ = new int[5]; //運気の変化(プラスの運気成分(旺気))
    private int[] minus_luck_ = new int[5];  //運気の変化(マイナスの運気成分(邪気))

    private List<FurnitureGrid> furniture_grid_ = new List<FurnitureGrid>(); //FurnitureGrid.csで実装されているクラスのリスト(最大50)
    private List<int> ignore_index_ = new List<int>(); //この中に入った家具グリッドは評価から除外される (2018年 2月15日)


    private bool is_finished_game_; //ゲームが終わったかどうかのフラグ (ture = ゲーム終了 false = ゲーム終了せず)
    private int advaice_mode_; //アドバイスモード(0 = 仕事運重視，1 = 人気運重視，2 = 健康運重視，3 = 金運重視，4 = 恋愛運重視, 5 = デフォルト(ノルマ重視))



    //*******************************************************************************************************************************************************************************************

    //五行木取得用
    public int elements_wood()
    {
        return elements_[0];
    }

    //五行火取得用
    public int elements_fire()
    {
        return elements_[1];
    }

    //五行土取得用
    public int elements_earth()
    {
        return elements_[2];
    }

    //五行金取得用
    public int elements_metal()
    {
        return elements_[3];
    }

    //五行水取得用
    public int elements_water()
    {
        return elements_[4];
    }

    //陰陽取得用(そういえば実装忘れてましたね…  2018年2月15日実装)
    public int yin_yang()
    {
        return yin_yang_;
    }


    //気の強さ取得用
    public int energy_strength()
    {
        return energy_strength_;
    }

    //仕事運(取得用)
    public int work_luck()
    {
        return luck_[0];
    }

    //人気運(取得用)
    public int popular_luck()
    {
        return luck_[1];
    }

    //健康運(取得用)
    public int health_luck()
    {
        return luck_[2];
    }

    //金運(取得用)
    public int economic_luck()
    {
        return luck_[3];
    }

    //恋愛運(取得用)
    public int love_luck()
    {
        return luck_[4];
    }

    //総合運(取得用)
    public int all_luck()
    {
        return all_luck_;
    }

    //仕事運ノルマ(取得用)
    public int work_norma()
    {
        return norma_luck_[0];
    }

    //人気運ノルマ(取得用)
    public int popular_norma()
    {
        return norma_luck_[1];
    }

    //健康運ノルマ(取得用)
    public int health_norma()
    {
        return norma_luck_[2];
    }

    //金運ノルマ(取得用)
    public int economic_norma()
    {
        return norma_luck_[4];
    }

    //恋愛運ノルマ(取得用)
    public int love_norma()
    {
        return norma_luck_[5];
    }

    //総合運ノルマ(取得用)
    public int all_norma()
    {
        return all_norma_;
    }

    //部屋の種類取得用 (そういえば実装忘れてましたね…  2018年2月15日実装)
    public Room room_role()
    {
        return room_role_;
    }

    //部屋の方向取得用 (そういえば実装忘れてましたね…  2018年2月15日実装)
    public Direction room_direction()
    {
        return room_direction_;
    }


    //コメント(取得用)
    public List<string> comment()
    {
        return comment_;
    }

    public Text hint_text_;
    private bool no_hint = false;

    public void Comment_Text(int num)
    {
        if (is_finished_game_ == true)
        {
            DataManager.set_comment(comment_);
        }
        else
        {
            if (no_hint == false)
            {
                hint_text_.text = comment_[num];
            }
            else
            {
                hint_text_.text = "自力で頑張れ!";
            }
        }
    }

    //***************************************************************************************************************
    //UI表示

    public Text[] elements_text_ = new Text[5];

    public GameObject Gage_Value_yin;
    public GameObject Gage_Value_yang;
    private int yin_yang_max_ = 300;
    private int yin_yang_min_ = -300;

    public GameObject[] Gage_Value_ = new GameObject[5];
    public GameObject[] Gage_Max_ = new GameObject[5];
    private int all_luck_min_ = -500;

    public GameObject game_shikigami;
    public Animator attack_shikigami;

    public void UpdateElementsText()
    {
        int max = 0;
        int compare = 0;

        //五行テキスト 
        for (int i = 0; i < elements_.Length; i++)
        {
            elements_text_[i].text = elements_[i].ToString();

            if (compare < elements_[i])
            {
                max = i;
                compare = elements_[i];
            }
        }

        if (max == 0)
        {
            game_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/wood/game");
        }
        else if (max == 1)
        {
            game_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/fire/game");
        }
        else if (max == 2)
        {
            game_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/earth/game");
        }
        else if (max == 3)
        {
            game_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/metal/game");
        }
        else if (max == 4)
        {
            game_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/water/game");
        }

        attack_shikigami.SetInteger("Element", max);
        DataManager.GetComponent<DataManager>().set_advaice_mode(max);

        //陰陽ゲージ
        float yin_yang_temp_ = yin_yang_;

        yin_yang_temp_ += (-yin_yang_min_);
        yin_yang_temp_ /= yin_yang_max_ + (-yin_yang_min_);

        Gage_Value_yang.GetComponent<RectTransform>().localScale = new Vector3(yin_yang_temp_, 1, 1);

        if (yin_yang_temp_ >= 1)
        {
            Gage_Value_yang.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        else if (yin_yang_temp_ <= 0)
        {
            Gage_Value_yang.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
        }

        Gage_Value_yin.GetComponent<RectTransform>().localScale = new Vector3(1 - Gage_Value_yang.GetComponent<RectTransform>().localScale.x, 1, 1);

        //運勢ゲージ
        int count = 0;

        for (int i = 0; i < luck_.Length; i++)
        {
            float luck_temp_ = luck_[i];

            luck_temp_ += (-all_luck_min_);
            luck_temp_ /= norma_luck_[i] + (-all_luck_min_);

            Gage_Value_[i].GetComponent<RectTransform>().localScale = new Vector3(luck_temp_, 1, 1);

            if (luck_temp_ >= 1)
            {
                count++;

                Gage_Value_[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                if (Gage_Max_[i].activeSelf == false)
                {
                    Gage_Max_[i].SetActive(true);
                    DataManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/gage_max"));
                }
            }
            else
            {
                if (luck_temp_ <= 0)
                {
                    Gage_Value_[i].GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
                }

                if (Gage_Max_[i].activeSelf == true)
                {
                    Gage_Max_[i].SetActive(false);
                    //DataManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/gage_max"));
                }
            }
        }

        if (count == luck_.Length)
        {
            GameObject.Find("LevelManager").GetComponent<LevelManager>().FinishGame(true);
        }
    }

    //*******************************************************************************************************************************************************************************************

    private DataManager DataManager;
    private Grid_Manager Grid_Manager;

    //初期化関数(この関数は最初に1回だけ実行すること)
    public void Init(Room room_role, Direction room_direction, int[] norma_luck, int advice_mode, List<FurnitureGrid> furniture_grid)
    {
        DataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        Grid_Manager = GameObject.Find("Grid_Manager").GetComponent<Grid_Manager>();

        room_role_ = room_role;
        room_direction_ = room_direction;
        norma_luck_ = norma_luck;
        furniture_grid_ = furniture_grid;

        if (advice_mode == 0)
        {
            set_norma(300, 300, 300, 300, 300, 300);
        }
        else if (advice_mode == 1)
        {
            set_norma(400, 400, 400, 400, 400, 400);
        }
        else if (advice_mode == 2)
        {
            set_norma(500, 500, 500, 500, 500, 500);
        }
        else if (advice_mode == 3)
        {
            set_norma(300, 300, 300, 300, 300, 300);
            no_hint = true;
        }
        else if (advice_mode == 4)
        {
            set_norma(500, 500, 500, 500, 500, 500);
            no_hint = true;
        }

        advaice_mode_ = 5;

        //エラーグリッド無視
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (furniture_grid_[i].furniture_grid().GetComponent<GridError>().errored())
            {
                ignore_index_.Add(i);
            }
        }

        ////特性を数えるため実装(2018年 2月15日実装)
        //for (int i = 0; i < furniture_grid.Count; ++i)
        //{
        //    //エラー家具を無視する処理
        //    if (IsIgnored(i))
        //    {
        //        continue;
        //    }
        //    CountCharacteristic(furniture_grid_[i]);
        //}

        is_finished_game_ = false;

        for (int i = 0; i < 5; ++i)
        {
            split_elements_[i] = new int[8];
            sosho_buffer_[i] = new int[8];
            sokoku_buffer_[i] = new int[8];
        }

        EvaluationTotal();
        UpdateElementsText();
        Comment_Text(0);
    }

    //ノルマセット関数
    public void set_norma(int work_norma, int popular_norma, int health_norma, int economic_norma, int love_norma, int all_norma)
    {
        norma_luck_[0] = work_norma;
        norma_luck_[1] = popular_norma;
        norma_luck_[2] = health_norma;
        norma_luck_[3] = economic_norma;
        norma_luck_[4] = love_norma;
        all_norma_ = all_norma;
    }

    //*******************************************************************************************************************************************************************************************

    //家具の更新関数
    //furniture_grid = 変更，更新する家具グリッド
    public void UpdateGrid(List<FurnitureGrid> furniture_grid)
    {
        furniture_grid_ = furniture_grid;

        //エラーグリッド無視
        ignore_index_.Clear();
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (furniture_grid_[i].furniture_grid().GetComponent<GridError>().errored())
            {
                ignore_index_.Add(i);
            }
        }

        //特性を数えるため実装(2018年 2月15日実装)
        characteristic_number_.Clear();
    }

    //is_finised_gameに値をセット
    public void set_is_finishedGame(bool is_finished_game)
    {
        is_finished_game_ = is_finished_game;
    }

    //advaice_mode_に値をセット
    public void set_advaice_mode(int advaice_mode)
    {
        if (advaice_mode < 0 || advaice_mode > 5)
        {
            Debug.Log("アドバイスモードの設定がおかしいのでデフォルトにします．");
        }
        advaice_mode_ = advaice_mode;
    }

    //*******************************************************************************************************************************************************************************************

    //総合評価関数(評価の一連の流れ)
    public void EvaluationTotal()
    {
        for (int i = 0; i < 5; ++i)
        {
            elements_[i] = 0;
            luck_[i] = 0;
            plus_luck_[i] = 0;
            minus_luck_[i] = 0;

            for (int j = 0; j < 8; ++j)
            {
                split_elements_[i][j] = 0;
                sosho_buffer_[i][j] = 0;
                sokoku_buffer_[i][j] = 0;
            }

        }

        for (int i = 0; i < 8; ++i)
        {
            split_yin_yang_[i] = 0;
        }

        yin_yang_ = 0;
        energy_strength_ = 0;
        all_luck_ = 0;
        comment_.Clear(); //コメントの初期化

        EvaluationItem();
        EvaluationRoom();
        EvaluationDirection();
        EvaluationSoshoSokoku();
        EvaluationFiveElementsMultiply();
        for (int i = 0; i < 8; ++i)
        {
            split_yin_yang_[i] += split_elements_[1][i];
            split_yin_yang_[i] += split_elements_[2][i] / 2;
            split_yin_yang_[i] -= split_elements_[3][i] / 2;
            split_yin_yang_[i] -= split_elements_[4][i];
        }
        EvaluationYinYangMultiply();


        //ここから五行の気の強さ, 陰陽加算
        for (int i = 0; i < 5; ++i)
        {
            elements_[i] += split_elements_[i].Sum();

            if (elements_[i] > limit_elements_)
            {
                energy_strength_ += limit_elements_;
            }
            else
            {
                energy_strength_ += elements_[i];
            }
        }
        yin_yang_ += split_yin_yang_.Sum();
        //ここまで五行の気の強さ, 陰陽加算

        FortuneItem();
        FortuneColor();
        FortuneMaterial();
        FortunePattern();
        FortuneForm();
        FortuneCharacteristic();

        FortuneRoom();
        FortuneDirection();
        FortuneSplitDirection();
        FortuneFiveElement();

        //陰陽による運勢補正
        if (yin_yang_ < limit_yin_)
        {
            int yin_minus_luck = -(yin_yang_ - limit_yin_);
            minus_luck_[0] += yin_minus_luck;
            minus_luck_[1] += yin_minus_luck;
            minus_luck_[2] += yin_minus_luck;
            minus_luck_[4] += yin_minus_luck;

            //陰気が強すぎて, 仕事，人気，健康，恋愛，下がる
            if (yin_minus_luck > 100)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.OverYin, 100, 100, 100, 0, 100, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.OverYin, yin_minus_luck, yin_minus_luck, yin_minus_luck, 0, yin_minus_luck, AdviceType.ElementGame));
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.OverYin, yin_minus_luck, yin_minus_luck, yin_minus_luck, 0, yin_minus_luck, AdviceType.ElementEnd));
        }
        else if (yin_yang_ > limit_yang_)
        {
            int yang_minus_luck = (yin_yang_ - limit_yang_);
            minus_luck_[0] += yang_minus_luck;
            minus_luck_[2] += yang_minus_luck;
            minus_luck_[4] += yang_minus_luck;

            //陽気が強すぎて，仕事，健康，恋愛下がる
            if (yang_minus_luck > 100)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.OverYang, 100, 0, 100, 0, 100, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.OverYang, yang_minus_luck, 0, yang_minus_luck, 0, yang_minus_luck, AdviceType.ElementGame));
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.OverYang, yang_minus_luck, 0, yang_minus_luck, 0, yang_minus_luck, AdviceType.ElementEnd));
        }

        FortuneLast();

        for (int i = 0; i < 5; ++i)
        {
            luck_[i] = plus_luck_[i] - minus_luck_[i];
        }

        all_luck_ = luck_[0] + luck_[1] + luck_[2] + luck_[3] + luck_[4];

        if (is_finished_game_)
        {
            Comment();
        }
        else
        {
            CommentMini();
        }

    }

    //*******************************************************************************************************************************************************************************************

    //アイテム評価関数(アイテムごとの五行と陰陽を加算)
    private void EvaluationItem()
    {
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.North)
            {
                split_elements_[0][0] += furniture_grid_[i].elements_wood();
                split_elements_[1][0] += furniture_grid_[i].elements_fire();
                split_elements_[2][0] += furniture_grid_[i].elements_earth();
                split_elements_[3][0] += furniture_grid_[i].elements_metal();
                split_elements_[4][0] += furniture_grid_[i].elements_water();
                split_yin_yang_[0] += furniture_grid_[i].yin_yang();
            }
            else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.NorthEast)
            {
                split_elements_[0][1] += furniture_grid_[i].elements_wood();
                split_elements_[1][1] += furniture_grid_[i].elements_fire();
                split_elements_[2][1] += furniture_grid_[i].elements_earth();
                split_elements_[3][1] += furniture_grid_[i].elements_metal();
                split_elements_[4][1] += furniture_grid_[i].elements_water();
                split_yin_yang_[1] += furniture_grid_[i].yin_yang();
            }
            else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.East)
            {
                split_elements_[0][2] += furniture_grid_[i].elements_wood();
                split_elements_[1][2] += furniture_grid_[i].elements_fire();
                split_elements_[2][2] += furniture_grid_[i].elements_earth();
                split_elements_[3][2] += furniture_grid_[i].elements_metal();
                split_elements_[4][2] += furniture_grid_[i].elements_water();
                split_yin_yang_[2] += furniture_grid_[i].yin_yang();
            }
            else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.SouthEast)
            {
                split_elements_[0][3] += furniture_grid_[i].elements_wood();
                split_elements_[1][3] += furniture_grid_[i].elements_fire();
                split_elements_[2][3] += furniture_grid_[i].elements_earth();
                split_elements_[3][3] += furniture_grid_[i].elements_metal();
                split_elements_[4][3] += furniture_grid_[i].elements_water();
                split_yin_yang_[3] += furniture_grid_[i].yin_yang();
            }
            else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.South)
            {
                split_elements_[0][4] += furniture_grid_[i].elements_wood();
                split_elements_[1][4] += furniture_grid_[i].elements_fire();
                split_elements_[2][4] += furniture_grid_[i].elements_earth();
                split_elements_[3][4] += furniture_grid_[i].elements_metal();
                split_elements_[4][4] += furniture_grid_[i].elements_water();
                split_yin_yang_[4] += furniture_grid_[i].yin_yang();
            }
            else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.SouthWest)
            {
                split_elements_[0][5] += furniture_grid_[i].elements_wood();
                split_elements_[1][5] += furniture_grid_[i].elements_fire();
                split_elements_[2][5] += furniture_grid_[i].elements_earth();
                split_elements_[3][5] += furniture_grid_[i].elements_metal();
                split_elements_[4][5] += furniture_grid_[i].elements_water();
                split_yin_yang_[5] += furniture_grid_[i].yin_yang();
            }
            else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.West)
            {
                split_elements_[0][6] += furniture_grid_[i].elements_wood();
                split_elements_[1][6] += furniture_grid_[i].elements_fire();
                split_elements_[2][6] += furniture_grid_[i].elements_earth();
                split_elements_[3][6] += furniture_grid_[i].elements_metal();
                split_elements_[4][6] += furniture_grid_[i].elements_water();
                split_yin_yang_[6] += furniture_grid_[i].yin_yang();
            }
            else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.NorthWest)
            {
                split_elements_[0][7] += furniture_grid_[i].elements_wood();
                split_elements_[1][7] += furniture_grid_[i].elements_fire();
                split_elements_[2][7] += furniture_grid_[i].elements_earth();
                split_elements_[3][7] += furniture_grid_[i].elements_metal();
                split_elements_[4][7] += furniture_grid_[i].elements_water();
                split_yin_yang_[7] += furniture_grid_[i].yin_yang();
            }

            if (furniture_grid_[i].material_type().IndexOf(FurnitureGrid.MaterialType.ArtificialFoliage) >= 0
                || furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Clutter) >= 0)
            {
                for (int j = 0; j < 8; ++j)
                {
                    split_yin_yang_[j] -= 125;
                }
            }
        }
    }

    //部屋評価関数(基本五行と陰陽のみ)
    private void EvaluationRoom()
    {

        if (room_role_ == Room.Bedroom)
        {

        }
        else if (room_role_ == Room.Living)
        {
            //土の気をもつ
            for (int i = 0; i < 8; ++i)
            {
                split_elements_[2][i] += 12;
            }
        }
        else if (room_role_ == Room.Workroom)
        {
            //木の気をもつ
            for (int i = 0; i < 8; ++i)
            {
                split_elements_[0][i] += 12;
            }
        }
        else
        {
            Debug.Log("そのような部屋は存在しない");
        }
    }

    //**************************************************************************************************************************************************************************************************

    //方位評価関数(部屋の)
    private void EvaluationDirection()
    {
        if (room_direction_ == Direction.North)
        {
            //北は水の気が強い
            for (int i = 0; i < 8; ++i)
            {
                split_elements_[4][i] += 25;
            }
        }
        else if (room_direction_ == Direction.NorthEast)
        {
            //北東は土の気が強い(山)
            for (int i = 0; i < 8; ++i)
            {
                split_elements_[2][i] += 25;
            }
        }
        else if (room_direction_ == Direction.East)
        {
            //東は木の気が強い(若木)
            for (int i = 0; i < 8; ++i)
            {
                split_elements_[0][i] += 25;
            }
        }
        else if (room_direction_ == Direction.SouthEast)
        {
            //南東は木の気が強い(大木)
            for (int i = 0; i < 8; ++i)
            {
                split_elements_[0][i] += 25;
            }
        }
        else if (room_direction_ == Direction.South)
        {
            //南は火の気が強い
            for (int i = 0; i < 8; ++i)
            {
                split_elements_[1][i] += 25;
            }
        }
        else if (room_direction_ == Direction.SouthWest)
        {
            //南西は土の気が強い
            for (int i = 0; i < 8; ++i)
            {
                split_elements_[2][i] += 25;
            }
        }
        else if (room_direction_ == Direction.West)
        {
            //西は金の気が強い
            for (int i = 0; i < 8; ++i)
            {
                split_elements_[3][i] += 25;
            }
        }
        else if (room_direction_ == Direction.NorthWest)
        {
            //北西は金の気が強い
            for (int i = 0; i < 8; ++i)
            {
                split_elements_[3][i] += 25;
            }
        }
        else
        {
            Debug.Log("そのような方位は存在しない");
        }
    }

    //五行評価関数(内部的に陰陽も変化)
    private void EvaluationSoshoSokoku()
    {
        //ここから相生の処理
        int[][] sosho_stock = new int[5][];
        for (int i = 0; i < 5; ++i)
        {
            sosho_stock[i] = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        for (int j = 0; j < 8; ++j)
        {
            for (int i = 0; i < 5; ++i)
            {
                //同じ方位
                if (split_elements_[i][j] / 2 <= split_elements_[(i + 1) % 5][j])
                {
                    sosho_stock[(i + 1) % 5][j] += split_elements_[i][j] / 2;
                    sosho_stock[i][j] -= split_elements_[i][j] / 4;
                }
                else
                {
                    sosho_stock[(i + 1) % 5][j] += split_elements_[(i + 1) % 5][j];
                    sosho_stock[i][j] -= split_elements_[(i + 1) % 5][j] / 2;
                }

                //時計隣
                if (split_elements_[i][j] / 4 <= split_elements_[(i + 1) % 5][(j + 1) % 8])
                {
                    sosho_stock[(i + 1) % 5][(j + 1) % 8] += split_elements_[i][j] / 4;
                    sosho_stock[i][j] -= split_elements_[i][j] / 8;
                }
                else
                {
                    sosho_stock[(i + 1) % 5][(j + 1) % 8] += split_elements_[(i + 1) % 5][(j + 1) % 8];
                    sosho_stock[i][j] -= split_elements_[(i + 1) % 5][(j + 1) % 8] / 2;
                }

                //反時計隣
                if (split_elements_[i][j] / 4 <= split_elements_[(i + 1) % 5][(j + 7) % 8])
                {
                    sosho_stock[(i + 1) % 5][(j + 7) % 8] += split_elements_[i][j] / 4;
                    sosho_stock[i][j] -= split_elements_[i][j] / 8;
                }
                else
                {
                    sosho_stock[(i + 1) % 5][(j + 7) % 8] += split_elements_[(i + 1) % 5][(j + 7) % 8];
                    sosho_stock[i][j] -= split_elements_[(i + 1) % 5][(j + 7) % 8] / 2;
                }
            }
        }

        for (int j = 0; j < 8; ++j)
        {
            for (int i = 0; i < 5; ++i)
            {
                if ((split_elements_[i][j] + sosho_stock[i][j]) < 0)
                {
                    split_elements_[i][j] = 0;
                    sosho_buffer_[i][j] -= split_elements_[i][j];
                }
                else
                {
                    split_elements_[i][j] += sosho_stock[i][j];
                    sosho_buffer_[i][j] += sosho_stock[i][j];
                }
            }
        }

        //ここから相克の処理
        int[][] sokoku_stock = new int[5][];
        for (int i = 0; i < 5; ++i)
        {
            sokoku_stock[i] = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        for (int j = 0; j < 8; ++j)
        {
            for (int i = 0; i < 5; ++i)
            {
                //同じ方位
                if (split_elements_[i][j] / 2 <= split_elements_[(i + 2) % 5][j])
                {
                    sokoku_stock[(i + 2) % 5][j] -= split_elements_[i][j] / 2;
                    sokoku_stock[i][j] -= split_elements_[i][j] / 4;
                }
                else
                {
                    sokoku_stock[(i + 2) % 5][j] -= split_elements_[(i + 2) % 5][j];
                    sokoku_stock[i][j] -= split_elements_[(i + 2) % 5][j] / 2;
                }

                //時計隣
                if (split_elements_[i][j] / 4 <= split_elements_[(i + 2) % 5][(j + 1) % 8])
                {
                    sokoku_stock[(i + 2) % 5][(j + 1) % 8] -= split_elements_[i][j] / 4;
                    sokoku_stock[i][j] -= split_elements_[i][j] / 8;
                }
                else
                {
                    sokoku_stock[(i + 2) % 5][(j + 1) % 8] -= split_elements_[(i + 2) % 5][(j + 1) % 8];
                    sokoku_stock[i][j] -= split_elements_[(i + 2) % 5][(j + 1) % 8] / 2;
                }

                //反時計隣
                if (split_elements_[i][j] / 4 <= split_elements_[(i + 2) % 5][(j + 7) % 8])
                {
                    sokoku_stock[(i + 2) % 5][(j + 7) % 8] -= split_elements_[i][j] / 4;
                    sokoku_stock[i][j] -= split_elements_[i][j] / 8;
                }
                else
                {
                    sokoku_stock[(i + 2) % 5][(j + 7) % 8] -= split_elements_[(i + 2) % 5][(j + 7) % 8];
                    sokoku_stock[i][j] -= split_elements_[(i + 2) % 5][(j + 7) % 8] / 2;
                }

            }

        }


        for (int j = 0; j < 8; ++j)
        {
            for (int i = 0; i < 5; ++i)
            {
                if ((split_elements_[i][j] + sokoku_stock[i][j]) < 0)
                {
                    split_elements_[i][j] = 0;
                    sokoku_buffer_[i][j] -= split_elements_[i][j];
                }
                else
                {
                    split_elements_[i][j] += sokoku_stock[i][j];
                    sokoku_buffer_[i][j] += sokoku_stock[i][j];
                }
            }
        }

    }

    //五行の掛け算補正(最初に五行，後に陰陽)(方角などはこちらに移動)
    private void EvaluationFiveElementsMultiply()
    {
        int[][] split_elements_stock = new int[5][];
        for (int i = 0; i < 5; ++i)
        {
            split_elements_stock[i] = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        }


        for (int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                split_elements_[i][j] += split_elements_stock[i][j];
                if (split_elements_[i][j] < 0)
                {
                    split_elements_[i][j] = 0;
                }

            }
        }
    }

    //陰陽の掛け算補正
    private void EvaluationYinYangMultiply()
    {
        int[] split_yin_yang_stock = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 }; //陰陽の気変化量

        //観葉植物による陰陽緩和
        int[] foliage_item = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }


            if ((furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.FoliagePlant)
                && (furniture_grid_[i].material_type().IndexOf(FurnitureGrid.MaterialType.ArtificialFoliage) < 0))
            {
                if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.North)
                {
                    ++foliage_item[0];
                }
                else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.NorthEast)
                {
                    ++foliage_item[1];
                }
                else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.East)
                {
                    ++foliage_item[2];
                }
                else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.SouthEast)
                {
                    ++foliage_item[3];
                }
                else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.South)
                {
                    ++foliage_item[4];
                }
                else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.SouthWest)
                {
                    ++foliage_item[5];
                }
                else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.West)
                {
                    ++foliage_item[6];
                }
                else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.NorthWest)
                {
                    ++foliage_item[7];
                }
            }
        }

        for (int i = 0; i < 8; ++i)
        {
            split_yin_yang_stock[(7 + i) % 8] -= split_yin_yang_[(7 + i) % 8] - (int)(split_yin_yang_[(7 + i) % 8] * System.Math.Pow((3 / 4), foliage_item[i]));
            split_yin_yang_stock[i] -= split_yin_yang_[i] - (int)(split_yin_yang_[i] * System.Math.Pow((1 / 2), foliage_item[i]));
            split_yin_yang_stock[(1 + i) % 8] -= split_yin_yang_[(1 + i) % 8] - (int)(split_yin_yang_[(1 + i) % 8] * System.Math.Pow((3 / 4), foliage_item[i]));
        }


        //白い家具による陰陽中和
        int white_item = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.White) >= 0)
            {
                ++white_item;
            }
        }

        for (int i = 0; i < 8; ++i)
        {
            split_yin_yang_stock[i] -= split_yin_yang_[i] - (int)(split_yin_yang_[i] * System.Math.Pow((9 / 10), foliage_item[i]));
        }

        for (int j = 0; j < 8; ++j)
        {
            //掛け算で正負判定しても良かったけどオバフロこわいので…
            if ((((split_yin_yang_[j] + split_yin_yang_stock[j]) >= 0) && (split_yin_yang_[j] >= 0))
                || (((split_yin_yang_[j] + split_yin_yang_stock[j]) <= 0) && (split_yin_yang_[j] <= 0)))
            {
                split_yin_yang_[j] += split_yin_yang_stock[j];
            }
            else
            {
                split_yin_yang_[j] = 0;
            }
        }
    }

    //**************************************************************************************************************************************************************************************************

    //アイテムによる運勢補正
    private void FortuneItem()
    {
        //家具の特性による評価

        //ベッド関連
        int bed_item = 0; //ベッドの数
        bool bed_south_to_north = false; //ベッドが南→北
        bool bed_south_to_east = false; //ベッドが南→東
        int bed_south_direction = 0; //南向きベッドの数
        bool bed_west_to_north = false; //ベッドが西→北
        bool bed_west_to_east = false; //ベッドが西→東
        int bed_west_direction = 0; //西向きベッドの数
        bool bed_north_to_east = false; //ベッドが北→東
        bool bed_east_to_north = false; //ベッドが東→北
        bool bed_connected = false; //ベッド同士がつながっている 恋愛運
        bool bed_gap_wall = false; //健康運下がる
        bool bed_to_door = false; //健康運下がる
        bool bed_near_window = false; //健康運下がる
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.Bed)
            {
                RaycastHit hit;

                //ベッドの枕の位置(安眠できるかどうかで，健康運，美容運にかかわる)
                if (furniture_grid_[i].up_direction() == Vector3.up)
                {
                    //ベッドが北枕だと安眠(仕事運，健康運が上がる)
                    plus_luck_[0] += 30;
                    plus_luck_[2] += 60;

                    bed_north_to_east = true;
                }
                else if (furniture_grid_[i].up_direction() == Vector3.down)
                {
                    //ベッドが南枕だと仕事運，健康運下がる
                    minus_luck_[0] += 40;
                    minus_luck_[2] += 60;

                    bed_south_to_north = true;
                    bed_south_to_east = true;
                    ++bed_south_direction;
                }
                else if (furniture_grid_[i].up_direction() == Vector3.right)
                {
                    //ベッドが東枕だと(仕事運，健康運上がる)
                    plus_luck_[0] += 40;
                    plus_luck_[2] += 30;
                    bed_east_to_north = true;
                }
                else if (furniture_grid_[i].up_direction() == Vector3.left)
                {
                    //ベッドが西枕だと(仕事運，健康運下がる)
                    minus_luck_[0] += 20;
                    minus_luck_[2] += 30;

                    bed_west_to_north = true;
                    bed_west_to_east = true;
                    ++bed_west_direction;
                }

                ////(シングル)ベッドをつなげるとダメ
                for (int j = 0; j < furniture_grid_.Count; ++j)
                {
                    //ベッド
                    if (furniture_grid_[i] != furniture_grid_[j])
                    {
                        if (furniture_grid_[j].furniture_type() == FurnitureGrid.FurnitureType.Bed)
                        {
                            Vector3 left_down_source = furniture_grid_[i].vertices((int)furniture_grid_[i].parameta(2));
                            Vector3 left_up_source = furniture_grid_[i].vertices((int)furniture_grid_[i].parameta(3));
                            Vector3 right_down_source = furniture_grid_[i].vertices((int)furniture_grid_[i].parameta(4));
                            Vector3 right_up_source = furniture_grid_[i].vertices((int)furniture_grid_[i].parameta(5));

                            Vector3 left_down_target = furniture_grid_[j].vertices((int)furniture_grid_[j].parameta(2));
                            Vector3 left_up_target = furniture_grid_[j].vertices((int)furniture_grid_[j].parameta(3));
                            Vector3 right_down_target = furniture_grid_[j].vertices((int)furniture_grid_[j].parameta(4));
                            Vector3 right_up_target = furniture_grid_[j].vertices((int)furniture_grid_[j].parameta(5));

                            if (((left_down_source == right_down_target) &&
                                (left_up_source == right_up_target)) ||
                                ((right_down_source == left_down_target) &&
                                (right_up_source == left_up_target)))
                            {
                                //つながっていると恋愛運下がる
                                minus_luck_[4] += 25;
                                bed_connected = true;
                                Debug.Log("つながっている");
                            }
                        }
                    }
                }

                //ベッドと壁の隙間ダメ
                bool left_down = false;
                bool left_up = false;
                bool right_down = false;
                bool right_up = false;

                for (int l = 0; l < furniture_grid_[i].vertices_number(); l++)
                {
                    for (int j = Grid_Manager.Grid_y_min(); j < Grid_Manager.Grid_y_max(); j++)
                    {
                        for (int k = Grid_Manager.Grid_x_min(); k < Grid_Manager.Grid_x_max(); k++)
                        {
                            if (Grid_Manager.point(k, j).wall == true)
                            {
                                float verticesx = furniture_grid_[i].vertices(l).x;
                                float verticesy = furniture_grid_[i].vertices(l).y;

                                float grid_x_min = Grid_Manager.point(k, j).pos.x - (Grid_Manager.Grid_density() / 2.0f);
                                float grid_y_min = Grid_Manager.point(k, j).pos.y - (Grid_Manager.Grid_density() / 2.0f);
                                float grid_x_max = Grid_Manager.point(k, j).pos.x + (Grid_Manager.Grid_density() / 2.0f);
                                float grid_y_max = Grid_Manager.point(k, j).pos.y + (Grid_Manager.Grid_density() / 2.0f);

                                if (grid_x_min < verticesx && verticesx < grid_x_max &&
                                    grid_y_min < verticesy && verticesy < grid_y_max)
                                {
                                    if (furniture_grid_[i].vertices(l) == furniture_grid_[i].vertices(furniture_grid_[i].parameta(2)))
                                    {
                                        left_down = true;
                                    }
                                    else if (furniture_grid_[i].vertices(l) == furniture_grid_[i].vertices(furniture_grid_[i].parameta(3)))
                                    {
                                        left_up = true;
                                    }
                                    else if (furniture_grid_[i].vertices(l) == furniture_grid_[i].vertices(furniture_grid_[i].parameta(4)))
                                    {
                                        right_down = true;
                                    }
                                    else if (furniture_grid_[i].vertices(l) == furniture_grid_[i].vertices(furniture_grid_[i].parameta(5)))
                                    {
                                        right_up = true;
                                    }
                                }
                            }
                        }
                    }
                }

                if ((left_down == true && left_up == true) || (right_down == true && right_up == true))
                {
                    //隙間なし
                    Debug.Log("隙間なし");
                }
                else
                {
                    //隙間ありだと健康運下がる
                    minus_luck_[2] += 40;
                    bed_gap_wall = true;
                    Debug.Log("隙間あり");
                }

                //ドアが正面            
                for (int j = 0; j < furniture_grid_.Count; ++j)
                {
                    //エラー家具を無視する処理
                    if (IsIgnored(i))
                    {
                        continue;
                    }

                    //ドア                   
                    if (Physics.Raycast(furniture_grid_[i].furniture_grid().transform.position, furniture_grid_[i].up_direction(), out hit))
                    {
                        if (hit.collider.gameObject.tag == "furniture_grid_door")
                        {
                            //健康運下がる
                            minus_luck_[2] += 40;
                            bed_to_door = true;
                        }
                    }
                }

                //窓の近くにベッドダメ
                for (int j = 0; j < furniture_grid_.Count; ++j)
                {
                    //エラー家具を無視する処理
                    if (IsIgnored(i))
                    {
                        continue;
                    }

                    //窓                  
                    if (Physics.Raycast(furniture_grid_[i].furniture_grid().transform.position, furniture_grid_[i].up_direction(), out hit))
                    {
                        if (hit.collider.gameObject.tag == "furniture_grid_window")
                        {
                            //近く
                            if (4.0f > Vector3.Distance(furniture_grid_[i].furniture_grid().transform.position, hit.collider.gameObject.transform.position))
                            {
                                //健康運下がる
                                minus_luck_[2] += 40;
                                bed_near_window = true;
                            }
                        }
                    }
                }
                ++bed_item;
            }
        }

        int bed_no_chemical = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if ((furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.Bed)
                && ((furniture_grid_[i].material_type().IndexOf(FurnitureGrid.MaterialType.Plastic) < 0)
                && (furniture_grid_[i].material_type().IndexOf(FurnitureGrid.MaterialType.ChemicalFibre) < 0)))
            {
                ++bed_no_chemical;
            }
        }
        plus_luck_[2] += 35 * bed_no_chemical;
        if ((bed_item - bed_no_chemical) > 0)
        {
            comment_flag_.Add(new CommentFlag(CommentIdentifier.BedNatural, 0, 0, 35, 0, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.BedNatural, 0, 0, 35 * (bed_no_chemical - bed_item), 0, 0, AdviceType.BonusGame));
        }
        if (bed_item != 0)
        {
            if (room_role_ == Room.Living)
            {
                //リビングにベッドは置くな(仕事，人気, 健康ダウン)
                minus_luck_[0] += 100;
                minus_luck_[1] += 100;
                minus_luck_[2] += 100;
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedLiving, 100, 100, 100, 0, 0, AdviceType.Bonus));
            }
            else if (room_role_ == Room.Workroom)
            {
                //仕事部屋にベッドは置くな(仕事，健康大幅ダウン)
                minus_luck_[0] += 150;
                minus_luck_[2] += 150;
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedWorkroom, 150, 0, 150, 0, 0, AdviceType.Bonus));
            }

            //ベッドの向き関連

            //北枕から東枕
            if (bed_north_to_east)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedNorthToEast, 10, 0, -30, 0, 0, AdviceType.BonusGame));
            }

            //東から北枕
            if (bed_east_to_north)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedEastToNorth, -10, 0, 30, 0, 0, AdviceType.BonusGame));
            }

            //南枕から北枕
            if (bed_south_to_north)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedSouthToNorth, 70, 0, 120, 0, 0, AdviceType.BonusGame));
            }

            //南枕から東枕
            if (bed_south_to_east)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedSouthToEast, 80, 0, 90, 0, 0, AdviceType.BonusGame));
            }

            //南枕の数
            if (bed_south_direction > 0)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedSouthDirection, bed_south_direction * 70, 0, bed_south_direction * 90, 0, 0, AdviceType.BonusEnd));
            }

            //西枕から北枕
            if (bed_west_to_north)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedWestToNorth, 50, 0, 90, 0, 0, AdviceType.BonusGame));
            }

            //西枕から東枕
            if (bed_west_to_east)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedWestToEast, 60, 0, 60, 0, 0, AdviceType.BonusGame));
            }

            //西枕の数
            if (bed_west_direction > 0)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedWestDirection, bed_west_direction * 50, 0, bed_west_direction * 60, 0, 0, AdviceType.BonusEnd));
            }


            {
                ////ベッド同士がくっついている場合(恋愛運下がる)
                //if (bed_connected > 0)
                //{
                //    comment_flag_.Add(new CommentFlag(CommentIdentifier.BedConnected, 0, 0, 0, 0, bed_connected));
                //    Debug.Log("BedConnected");
                //}

                ////ベッドと壁の間に隙間ある場合
                //if (bed_gap_wall > 0)
                //{
                //    comment_flag_.Add(new CommentFlag(CommentIdentifier.BedGapWall, 0, 0, bed_gap_wall, 0, 0));
                //    Debug.Log("BedGapWall");
                //}

                ////ドアがベッドに向けている場合
                //if (bed_to_door > 0)
                //{
                //    comment_flag_.Add(new CommentFlag(CommentIdentifier.BedToDoor, 0, 0, bed_to_door, 0, 0));
                //    Debug.Log("BedToDoor");
                //}

                ////窓がベッドの近くの場合
                //if (bed_near_window > 0)
                //{
                //    comment_flag_.Add(new CommentFlag(CommentIdentifier.BedNearWindow, 0, 0, bed_near_window, 0, 0));
                //    Debug.Log("BedNearWindow");
                //}
            }

            //ベッドが多すぎる．
            if (bed_item > limit_bed_)
            {
                int bed_over_buffer = 100 * (bed_item - limit_bed_);
                for (int i = 0; i < 5; ++i)
                {
                    minus_luck_[i] += bed_over_buffer;
                }
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedOver, 100, 100, 100, 100, 100, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedOver, bed_over_buffer, bed_over_buffer, bed_over_buffer, bed_over_buffer, bed_over_buffer, AdviceType.BonusEnd));
            }
        }
        else
        {
            if (room_role_ == Room.Bedroom)
            {
                //寝室にベッド置かないのは論外(健康運中心に大幅下がる)
                minus_luck_[0] += 50;
                minus_luck_[1] += 50;
                minus_luck_[2] += 300;
                minus_luck_[3] += 50;
                minus_luck_[4] += 50;
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BedNoBedroom, 50, 50, 300, 50, 50, AdviceType.Bonus));
            }
        }



        //タンス関連
        int cabinet_item = 0; //タンスの数
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.Cabinet)
            {
                ++cabinet_item;
            }
        }

        if (cabinet_item != 0)
        {
            //タンス置きすぎ
            if (cabinet_item > limit_cabinet_)
            {

                int cabinet_over_buffer = 100 * (cabinet_item - limit_cabinet_);
                for (int i = 0; i < 5; ++i)
                {
                    minus_luck_[i] += cabinet_over_buffer;
                }
                comment_flag_.Add(new CommentFlag(CommentIdentifier.CabinetOver, 100, 100, 100, 100, 100, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.CabinetOver, cabinet_over_buffer, cabinet_over_buffer, cabinet_over_buffer, cabinet_over_buffer, cabinet_over_buffer, AdviceType.BonusEnd));
            }
        }
        else
        {

        }



        //カーペット関連
        int carpet_item = 0; //カーペットの数
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.Carpet)
            {
                ++carpet_item;
            }

        }

        if (carpet_item != 0)
        {
            //カーペット置きすぎ
            if (carpet_item > limit_carpet_)
            {
                int carpet_over_buffer = 100 * (carpet_item - limit_carpet_);
                for (int i = 0; i < 5; ++i)
                {
                    minus_luck_[i] += carpet_over_buffer;
                }
                comment_flag_.Add(new CommentFlag(CommentIdentifier.CarpetOver, 100, 100, 100, 100, 100, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.CarpetOver, carpet_over_buffer, carpet_over_buffer, carpet_over_buffer, carpet_over_buffer, carpet_over_buffer, AdviceType.BonusEnd));

            }
        }
        else
        {

        }




        //机関連
        int desk_item = 0; //机の数
        int desk_no_north_east = 0; //仕事運上げる
        int desk_no_south = 0; //人気運上げる
        int desk_no_west = 0; //金運上げる
        int desk_front_window = 0; //仕事運下げる
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.Desk)
            {

                //北向き, または東向き
                if ((furniture_grid_[i].up_direction() == Vector3.up)
                    || (furniture_grid_[i].up_direction() == Vector3.right))
                {
                    //仕事運アップ
                    plus_luck_[0] += 50;
                    ++desk_no_south;
                    ++desk_no_west;
                }
                //西向き
                else if (furniture_grid_[i].up_direction() == Vector3.left)
                {
                    //金運アップ
                    plus_luck_[3] += 50;
                    ++desk_no_north_east;
                    ++desk_no_south;
                }
                //南向き
                else if (furniture_grid_[i].up_direction() == Vector3.down)
                {
                    //人気運アップ
                    plus_luck_[1] += 50;
                    ++desk_no_north_east;
                    ++desk_no_west;
                }

                //窓の正面
                RaycastHit hit;
                for (int j = 0; j < furniture_grid_.Count; ++j)
                {
                    //エラー家具を無視する処理
                    if (IsIgnored(j))
                    {
                        continue;
                    }

                    //窓                  
                    if (Physics.Raycast(furniture_grid_[i].furniture_grid().transform.position, furniture_grid_[i].up_direction(), out hit))
                    {
                        if (hit.collider.gameObject.tag == "furniture_grid_window")
                        {
                            //机の座席が窓に対面すると仕事運下がる
                            minus_luck_[0] += 30;
                            desk_front_window += 30;
                        }
                    }
                }
                ++desk_item;
            }
        }

        if (desk_item != 0)
        {
            if (room_role_ == Room.Bedroom)
            {
                //寝室に机は置くな(仕事，健康運ダウン)
                minus_luck_[0] += 150;
                minus_luck_[2] += 150;
                comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskBedroom, 150, 0, 150, 0, 0, AdviceType.Bonus));
            }
            else if (room_role_ == Room.Workroom)
            {
                //机が北, または東向きでない
                if (desk_no_north_east > 0)
                {
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskSouthToNorthEast, 50, -50, 0, 0, 0, AdviceType.BonusGame));
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskWestToNorthEast, 50, 0, 0, -50, 0, AdviceType.BonusGame));
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskNoNorthEast, 50 * desk_no_north_east, 0, 0, 0, 0, AdviceType.BonusEnd));
                }

                //机が南向きでない
                if (desk_no_south > 0)
                {
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskNorthEastToSouth, -50, 50, 0, 0, 0, AdviceType.BonusGame));
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskWestToSouth, 0, 50, 0, -50, 0, AdviceType.BonusGame));
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskNoSouth, 0, 50 * desk_no_north_east, 0, 0, 0, AdviceType.BonusEnd));
                }

                //机が西向きでない
                if (desk_no_west > 0)
                {
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskNorthEastToWest, -50, 0, 0, 50, 0, AdviceType.BonusGame));
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskSouthToWest, 0, -50, 0, 50, 0, AdviceType.BonusGame));
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskNoWest, 0, 0, 0, 50 * desk_no_north_east, 0, AdviceType.BonusEnd));
                }
            }

            ////窓と机が正面衝突
            //if (desk_front_window > 0)
            //{
            //    comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskFrontWindow, desk_front_window, 0));
            //    comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskFrontWindow, desk_front_window, 5));
            //    Debug.Log("DeskFrontWindow");
            //}

            //机置きすぎ
            if (desk_item > limit_desk_)
            {
                int desk_over_buffer = 100 * (desk_item - limit_desk_);
                for (int i = 0; i < 5; ++i)
                {
                    minus_luck_[i] += desk_over_buffer;
                }
                comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskOver, 100, 100, 100, 100, 100, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskOver, desk_over_buffer, desk_over_buffer, desk_over_buffer, desk_over_buffer, desk_over_buffer, AdviceType.BonusEnd));
            }
        }
        else
        {
            if (room_role_ == Room.Workroom)
            {
                //仕事部屋に机置かないのは論外(仕事運に大幅下がる)
                minus_luck_[0] += 300;
                comment_flag_.Add(new CommentFlag(CommentIdentifier.DeskNoWorkRoom, 300, 0, 0, 0, 0, AdviceType.Bonus));
            }
        }



        //観葉植物関連
        int foliage_item = 0; //観葉植物関連
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.FoliagePlant)
            {
                ++foliage_item;

            }

        }

        if (foliage_item != 0)
        {
            //観葉植物置きすぎ
            if (foliage_item > limit_foliage_)
            {
                int foliage_over_buffer = 100 * (foliage_item - limit_foliage_);
                for (int i = 0; i < 5; ++i)
                {
                    minus_luck_[i] += foliage_over_buffer;
                }
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FoliagePlantOver, 100, 100, 100, 100, 100, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FoliagePlantOver, foliage_over_buffer, foliage_over_buffer, foliage_over_buffer, foliage_over_buffer, foliage_over_buffer, AdviceType.BonusEnd));
            }
        }
        else
        {

        }



        //ランプ関連
        int lamp_item = 0; //ランプの数
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }

            if ((furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.CeilLamp)
                || (furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.DeskLamp))
            {
                ++lamp_item;
            }
        }

        if (lamp_item != 0)
        {
            //ランプ置きすぎ
            if (lamp_item > limit_lamp_)
            {
                int lamp_over_buffer = 100 * (lamp_item - limit_lamp_);
                for (int i = 0; i < 5; ++i)
                {
                    minus_luck_[i] += lamp_over_buffer;
                }
                comment_flag_.Add(new CommentFlag(CommentIdentifier.LampOver, 100, 100, 100, 100, 100, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.LampOver, lamp_over_buffer, lamp_over_buffer, lamp_over_buffer, lamp_over_buffer, lamp_over_buffer, AdviceType.BonusEnd));
            }
        }
        else
        {
            //ランプ0は論外(金運以外大幅ダウン) 改変候補
            minus_luck_[0] += 200;
            minus_luck_[1] += 200;
            minus_luck_[2] += 200;
            minus_luck_[4] += 200;
            comment_flag_.Add(new CommentFlag(CommentIdentifier.LampNo, 200, 200, 200, 0, 200, AdviceType.Bonus));
        }


        //ソファー関連
        int sofa_item = 0; //ソファーの数
        int sofa_split_west = 0; //仕事運，人気運，健康運上がる
        int sofa_to_TV = 0; //健康運上がる
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.Sofa)
            {
                RaycastHit hit;

                //西に配置
                if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.West)
                {
                    //東向き
                    if (furniture_grid_[i].up_direction() == new Vector3(-1, 0, 0))
                    {
                        //西にソファーをおき，座席を東に向けると仕事運，人気運，健康運アップ
                        plus_luck_[0] += 10;
                        plus_luck_[1] += 10;
                        plus_luck_[2] += 10;
                        plus_luck_[3] += 10;
                        plus_luck_[4] += 10;

                        for (int j = 0; j < furniture_grid_.Count; ++j)
                        {
                            //エラー家具を無視する処理
                            if (IsIgnored(i))
                            {
                                continue;
                            }

                            //テレビ
                            if ((furniture_grid_[j].furniture_type() == FurnitureGrid.FurnitureType.ConsumerElectronics) && (furniture_grid_[j].parameta(0) == 2))
                            {
                                //西向き
                                if (furniture_grid_[j].up_direction() == new Vector3(1, 0, 0))
                                {
                                    if (Physics.Raycast(furniture_grid_[i].furniture_grid().transform.position, furniture_grid_[i].up_direction(), out hit))
                                    {
                                        if (hit.collider.gameObject == furniture_grid_[j].furniture_grid())
                                        {
                                            //ソファーに対してテレビが東側にあれば仕事運アップ
                                            plus_luck_[0] += 50;

                                        }
                                        else
                                        {
                                            //ソファーに対してテレビが東側以外
                                            sofa_to_TV += 50;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //ソファー西で東に向く以外
                        ++sofa_split_west;
                    }
                }
                ++sofa_item;
            }
        }

        if (sofa_item != 0)
        {
            //西以外のソファーが一つでもある場合
            if (sofa_split_west > 0)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SofaSplitWest, 10, 10, 10, 10, 10, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SofaSplitWest, 10 * sofa_split_west, 10 * sofa_split_west, 10 * sofa_split_west, 10 * sofa_split_west, 10 * sofa_split_west, AdviceType.BonusEnd));
            }

            //ソファ置きすぎ
            if (sofa_item > limit_sofa_)
            {
                int sofa_over_buffer = 100 * (sofa_item - limit_sofa_);
                for (int i = 0; i < 5; ++i)
                {
                    minus_luck_[i] += sofa_over_buffer;
                }
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SofaOver, 100, 100, 100, 100, 100, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SofaOver, sofa_over_buffer, sofa_over_buffer, sofa_over_buffer, sofa_over_buffer, sofa_over_buffer, AdviceType.BonusEnd));
            }

        }
        else
        {
            if (room_role_ == Room.Living)
            {
                //リビングでソファー0は論外(仕事運，人気運，健康運ダウン)
                minus_luck_[0] += 50;
                minus_luck_[1] += 50;
                minus_luck_[2] += 150;
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SofaNoLiving, 50, 50, 150, 0, 0, AdviceType.Bonus));
            }
        }


        //テーブル関連
        int table_item = 0; //テーブルの数
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.Table)
            {
                ++table_item;
            }
        }

        if (table_item != 0)
        {
            //テーブル置きすぎ
            if (table_item > limit_table_)
            {
                int table_over_buffer = 100 * (table_item - limit_table_);
                for (int i = 0; i < 5; ++i)
                {
                    minus_luck_[i] += table_over_buffer;
                }
                comment_flag_.Add(new CommentFlag(CommentIdentifier.TableOver, 100, 100, 100, 100, 100, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.TableOver, table_over_buffer, table_over_buffer, table_over_buffer, table_over_buffer, table_over_buffer, AdviceType.BonusEnd));
            }
        }
        else
        {
            if (room_role_ == Room.Living)
            {
                //リビングでテーブル0は論外(仕事運，人気運，健康運ダウン)
                minus_luck_[0] += 50;
                minus_luck_[1] += 50;
                minus_luck_[2] += 150;
                comment_flag_.Add(new CommentFlag(CommentIdentifier.TableNoLiving, 50, 50, 150, 0, 0, AdviceType.Bonus));
            }
        }

        //家電関連
        int electronics_item = 0;
        int electronics_south = 0;
        int electronics_no_east = 0;
        int TV_no_to_west = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {

            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.ConsumerElectronics)
            {
                //南に配置
                if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.South)
                {
                    //仕事運 健康運 恋愛運下がる
                    minus_luck_[0] += 30;
                    minus_luck_[2] += 30;
                    minus_luck_[4] += 30;

                    ++electronics_south;
                }
                //東に配置
                else if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.East)
                {
                    //仕事運 人気運 健康運あがる
                    plus_luck_[0] += 20;
                    plus_luck_[1] += 15;
                    plus_luck_[2] += 15;

                    //テレビならば
                    if (furniture_grid_[i].parameta(0) == 2)
                    {
                        //西向き
                        if (furniture_grid_[i].up_direction() == new Vector3(-1, 0, 0))
                        {
                            plus_luck_[0] += 20;
                            plus_luck_[1] += 20;
                            plus_luck_[4] += 20;
                        }
                        else
                        {
                            ++TV_no_to_west;
                        }
                    }
                }
                else
                {
                    ++electronics_no_east;
                }
            }
        }

        if (electronics_item != 0)
        {
            //南に配置されている場合
            if (electronics_south > 0)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.ElectronicsSouth, 30, 0, 30, 0, 30, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.ElectronicsSouth, 30 * electronics_south, 0, 30 * electronics_south, 0, 30 * electronics_south, AdviceType.BonusEnd));
            }

            //東に配置されている場合
            if (electronics_no_east > 0)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.ElectronicsNoEast, 20, 15, 15, 0, 0, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.ElectronicsNoEast, 20 * electronics_no_east, 15 * electronics_no_east, 15 * electronics_no_east, 0, 0, AdviceType.BonusEnd));
            }

            if (TV_no_to_west > 0)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.TVNoToWest, 20, 15, 15, 0, 0, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.TVNoToWest, 20 * electronics_no_east, 15 * electronics_no_east, 15 * electronics_no_east, 0, 0, AdviceType.BonusEnd));
            }

            if (electronics_item > limit_electronics_)
            {
                int electronics_over_buffer = 100 * (electronics_item - limit_electronics_);
                for (int i = 0; i < 5; ++i)
                {
                    minus_luck_[i] += electronics_over_buffer;
                }
                comment_flag_.Add(new CommentFlag(CommentIdentifier.ElectronicsOver, 100, 100, 100, 100, 100, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.ElectronicsOver, electronics_over_buffer, electronics_over_buffer, electronics_over_buffer, electronics_over_buffer, electronics_over_buffer, AdviceType.BonusEnd));
            }

        }
        else
        {

        }


        //鏡関連
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            //エラー家具を無視する処理
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.Dresser)
            {
                //RaycastHit hit;

                ////南に配置
                //if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.South)
                //{
                //    luck_[1] += 10;
                //    luck_[4] += 5;
                //    luck_[2] += 5;
                //}

                ////丸い鏡
                //if (furniture_grid_[i].form_type()[0] == FurnitureGrid.FormType.Round)
                //{
                //    //全ての運気が1.2倍
                //    luck_[2] = (int)(luck_[2] * 1.2f);
                //    luck_[3] = (int)(luck_[3] * 1.2f);
                //    luck_[4] = (int)(luck_[4] * 1.2f);
                //    luck_[0] = (int)(luck_[0] * 1.2f);
                //    luck_[1] = (int)(luck_[1] * 1.2f);

                //    //リビング
                //    if (room_role_ == Room.Living)
                //    {
                //        //北の方角に置く
                //        if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.North)
                //        {
                //            //全体的に運気が上がる（特に金運）
                //        }
                //    }
                //}
                ////四角い鏡
                //if (furniture_grid_[i].form_type()[0] == FurnitureGrid.FormType.Square)
                //{
                //    //全ての運気が0.8倍
                //    luck_[2] = (int)(luck_[2] * 0.8f);
                //    luck_[3] = (int)(luck_[3] * 0.8f);
                //    luck_[4] = (int)(luck_[4] * 0.8f);
                //    luck_[0] = (int)(luck_[0] * 0.8f);
                //    luck_[1] = (int)(luck_[1] * 0.8f);

                //    //北の方角に置く
                //    if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.North)
                //    {
                //        //ダメ
                //    }

                //    //北東の方角に置く
                //    if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.NorthEast)
                //    {
                //        //良い
                //    }

                //    //窓の正面
                //    for (int j = 0; j < furniture_grid_.Count; ++j)
                //    {
                //        //窓
                //        if (furniture_grid_[j].furniture_type() == FurnitureGrid.FurnitureType.Window)
                //        {
                //            if (Physics.Raycast(furniture_grid_[i].furniture_grid().transform.position, furniture_grid_[i].up_direction(), out hit))
                //            {
                //                if (hit.collider.gameObject == furniture_grid_[j].furniture_grid())
                //                {
                //                    //ダメ
                //                }
                //            }
                //        }
                //    }
                //}
                ////長方形の鏡
                //if (furniture_grid_[i].form_type()[0] == FurnitureGrid.FormType.Rectangle)
                //{
                //    //東の方角に置く
                //    if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.East)
                //    {
                //        //良い
                //    }
                //    //南東の方角に置く
                //    if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.SouthEast)
                //    {
                //        //良い
                //    }
                //}
                ////
                //for (int j = 0; j < furniture_grid_.Count; ++j)
                //{
                //    //ベッドが正面(写っている)
                //    if (furniture_grid_[j].furniture_type() == FurnitureGrid.FurnitureType.bed)
                //    {
                //        if (Physics.Raycast(furniture_grid_[i].furniture_grid().transform.position, furniture_grid_[i].up_direction(), out hit))
                //        {
                //            if (hit.collider.gameObject == furniture_grid_[j].furniture_grid())
                //            {
                //                luck_[2] -= 20;
                //                luck_[4] -= 20;
                //                luck_[0] -= 5;
                //                luck_[1] -= 5;
                //                luck_[3] -= 5;
                //            }
                //        }
                //    }
                //    //鏡が正面(合わせ鏡)
                //    if (furniture_grid_[j].furniture_type() == FurnitureGrid.FurnitureType.dresser)
                //    {
                //        if (Physics.Raycast(furniture_grid_[i].furniture_grid().transform.position, furniture_grid_[i].up_direction(), out hit))
                //        {
                //            if (hit.collider.gameObject == furniture_grid_[j].furniture_grid())
                //            {
                //                luck_[2] -= 40;
                //                luck_[4] -= 40;
                //                luck_[0] -= 40;
                //                luck_[1] -= 40;
                //                luck_[3] -= 60;
                //            }
                //        }
                //    }
                //}

            }
        }

        if ((furniture_grid_.Count - ignore_index_.Count) < limit_furniture_few_)
        {
            //家具少なすぎ
            int furniture_few_buffer = 100 * (limit_furniture_few_ - (furniture_grid_.Count - ignore_index_.Count));
            for (int i = 0; i < 5; ++i)
            {
                minus_luck_[i] += furniture_few_buffer;
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.FurnitureFew, 100, 100, 100, 100, 100, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.FurnitureFew, furniture_few_buffer, furniture_few_buffer, furniture_few_buffer, furniture_few_buffer, furniture_few_buffer, AdviceType.BonusEnd));
        }
        else if ((furniture_grid_.Count - ignore_index_.Count) > limit_furniture_)
        {
            //家具多すぎ
            int furniture_over_buffer = 100 * ((furniture_grid_.Count - ignore_index_.Count) - limit_furniture_);
            for (int i = 0; i < 5; ++i)
            {
                minus_luck_[i] += furniture_over_buffer;
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.FurnitureOver, 100, 100, 100, 100, 100, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.FurnitureOver, furniture_over_buffer, furniture_over_buffer, furniture_over_buffer, furniture_over_buffer, furniture_over_buffer, AdviceType.BonusEnd));
        }

    }

    //色
    private void FortuneColor()
    {
        //まず家具の数を数えましょう
        int black_item = 0;
        int white_item = 0;
        int gray_item = 0;
        int red_item = 0;
        int pink_item = 0;
        int blue_item = 0;
        int lightblue_item = 0;
        int orange_item = 0;
        int silver_item = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Black) >= 0)
            {
                ++black_item;
            }

            if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Gray) >= 0)
            {
                ++gray_item;
            }

            if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Red) >= 0)
            {
                ++red_item;
            }

            if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Pink) >= 0)
            {
                ++pink_item;
            }

            if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Blue) >= 0)
            {
                ++blue_item;
            }

            if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.LightBlue) >= 0)
            {
                ++lightblue_item;
            }

            if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Orange) >= 0)
            {
                ++orange_item;
            }
        }

        //黒関連
        bool black_blue = false;
        bool warm_green = false;
        int[] black_plus_stock = new int[5] { 0, 0, 0, 0, 0 };
        int[] black_minus_stock = new int[5] { 0, 0, 0, 0, 0 };
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if ((furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Black) >= 0)
                || (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Blue) >= 0))
            {
                black_blue = true;
            }

            if ((furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Warm) >= 0)
                 || (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Green) >= 0))
            {
                warm_green = true;
            }

        }

        plus_luck_[3] += 10 * black_item;
        comment_flag_.Add(new CommentFlag(CommentIdentifier.BlackInteger, 0, 0, 0, 10, 0, AdviceType.BonusGame));
        comment_flag_.Add(new CommentFlag(CommentIdentifier.BlackInteger, 0, 0, 0, 10 * (limit_furniture_ - black_item), 0, AdviceType.BonusEnd));
        if (black_blue)
        {
            //青，黒い家具を置くときは緑の家具, 温かみと合わせましょう
            if (warm_green == false)
            {
                minus_luck_[1] += 50;
                minus_luck_[2] += 25;
                minus_luck_[4] += 25;

                for (int i = 0; i < furniture_grid_.Count; ++i)
                {
                    if (IsIgnored(i))
                    {
                        continue;
                    }

                    if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Blue) >= 0)
                    {
                        black_minus_stock[1] += 50;
                        black_minus_stock[2] += 25;
                        black_minus_stock[4] += 25;
                        break;
                    }
                }
                comment_flag_.Add(new CommentFlag(CommentIdentifier.BlackNoGreemWarm, 0, 50, 25, 0, 25, AdviceType.Bonus));
            }
        }


        //灰色関連
        int gray_northwest = 0;
        int gray_no_northwest = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if (((furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Gray) >= 0)
                || (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.DarkGray) >= 0))
                || (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Silver) >= 0))
            {
                if (room_direction_ == Direction.NorthWest || furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.NorthWest)
                {
                    plus_luck_[0] += 15;
                    plus_luck_[4] += 10;

                    black_plus_stock[0] += 15;
                    black_plus_stock[1] += 10;

                    ++gray_northwest;
                }
                ++gray_no_northwest;
            }
        }

        if (room_direction_ == Direction.NorthWest)
        {
            //北西の部屋にはグレー、ダークグレー、シルバーがいいです
            comment_flag_.Add(new CommentFlag(CommentIdentifier.GrayNorthWest, 15, 0, 0, 10, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.GrayNorthWest, 15 * (limit_furniture_ - gray_northwest), 0, 0, 10 * (limit_furniture_ - gray_northwest), 0, AdviceType.BonusEnd));
        }
        else
        {
            //グレー, 銀色，ダークグレーの家具は北西に
            comment_flag_.Add(new CommentFlag(CommentIdentifier.GraySplitNorthWest, 15, 0, 0, 10, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.GraySplitNorthWest, 15 * gray_no_northwest, 0, 0, 10 * gray_no_northwest, 0, AdviceType.BonusEnd));
        }


        //赤関連
        if (red_item > 0)
        {
            plus_luck_[0] += 40;
            plus_luck_[2] += 20;

            black_plus_stock[0] += 40;
            black_plus_stock[2] += 20;

            if (red_item > limit_red_color_)
            {
                minus_luck_[0] += 20 * (limit_red_color_ - red_item);
                minus_luck_[2] += 20 * (limit_red_color_ - red_item);

                black_minus_stock[0] += 20;
                black_minus_stock[2] += 20;

                comment_flag_.Add(new CommentFlag(CommentIdentifier.RedOver, 20, 0, 20, 0, 0, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.RedOver, 20 * (limit_red_color_ - red_item), 0, 20 * (limit_red_color_ - red_item), 0, 0, AdviceType.BonusEnd));
            }
        }
        else
        {
            comment_flag_.Add(new CommentFlag(CommentIdentifier.RedOne, 40, 0, 20, 0, 0, AdviceType.Bonus));
        }


        //ピンク関連
        int pink_north = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }
            if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Pink) >= 0)
            {
                if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.North)
                {
                    ++pink_north;
                }
            }
        }

        if (room_role_ == Room.Bedroom)
        {
            plus_luck_[4] += 15 * pink_item;
            black_plus_stock[4] += 15 * pink_item;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.PinkBed, 0, 0, 0, 0, 15, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.PinkBed, 0, 0, 0, 0, 15 * (limit_furniture_ - pink_item), AdviceType.BonusEnd));
        }

        if (room_direction_ == Direction.North)
        {
            //北の部屋はピンクがいいです
            plus_luck_[4] += 25 * pink_item;
            black_plus_stock[4] += 25 * pink_item;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.PinkNorth, 0, 0, 0, 0, 25, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.PinkNorth, 0, 0, 0, 0, 25 * (limit_furniture_ - pink_item), AdviceType.BonusEnd));
        }
        else
        {
            plus_luck_[4] += 25 * pink_north;
            black_plus_stock[4] += 25 * pink_north;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.PinkSplitNorth, 0, 0, 0, 0, 25, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.PinkSplitNorth, 0, 0, 0, 0, 25 * (pink_item - pink_north), AdviceType.BonusEnd));
        }

        if (pink_item > 0)
        {
            plus_luck_[4] += 60;
            black_plus_stock[4] += 60;

            if (orange_item > 0)
            {
                plus_luck_[4] += 50;
                black_plus_stock[4] += 50;
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.PinkColorNoOrange, 0, 0, 0, 0, 50, AdviceType.Bonus));
            }
        }
        else
        {
            comment_flag_.Add(new CommentFlag(CommentIdentifier.PinkColorOne, 0, 0, 0, 0, 60, AdviceType.Bonus));
        }


        //青関連
        int blue_lightblue = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }
            if ((furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Blue) >= 0)
                || (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.LightBlue) >= 0))
            {
                ++blue_lightblue;
            }

        }
        plus_luck_[2] += 10 * blue_lightblue;
        black_plus_stock[2] += 10 * blue_lightblue;
        comment_flag_.Add(new CommentFlag(CommentIdentifier.BlueInteger, 0, 0, 10, 0, 0, AdviceType.BonusGame));
        comment_flag_.Add(new CommentFlag(CommentIdentifier.BlueInteger, 0, 0, 10 * (limit_furniture_ - blue_lightblue), 0, 0, AdviceType.BonusEnd));
        if (blue_item > 0)
        {
            plus_luck_[0] += 50;
            black_plus_stock[0] += 50;
        }
        else
        {
            comment_flag_.Add(new CommentFlag(CommentIdentifier.BlueColorOne, 50, 0, 0, 0, 0, AdviceType.Bonus));
        }

        //水色関連
        if (lightblue_item > 0)
        {
            if (orange_item > 0)
            {
                plus_luck_[2] += 50;
                black_plus_stock[2] += 50;
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.LightBlueColorNoOrange, 0, 0, 50, 0, 0, AdviceType.Bonus));
            }
        }

        //オレンジ関連
        int orange_southeast = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Orange) >= 0)
            {
                if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.SouthEast)
                {
                    ++orange_southeast;
                }
            }
        }

        if (room_direction_ == Direction.SouthEast)
        {
            //南東の部屋オレンジいい
            plus_luck_[1] += 12 * orange_item;
            plus_luck_[4] += 13 * orange_item;

            black_plus_stock[1] += 12 * orange_item;
            black_plus_stock[4] += 13 * orange_item;
            comment_flag_.Add(new CommentFlag(CommentIdentifier.OrangeSouthEast, 0, 12, 13, 0, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.OrangeSouthEast, 0, 12 * (limit_furniture_ - orange_item), 13 * (limit_furniture_ - orange_item), 0, 0, AdviceType.BonusEnd));
        }
        else
        {
            plus_luck_[1] += 12 * orange_southeast;
            plus_luck_[4] += 13 * orange_southeast;

            black_plus_stock[1] += 12 * orange_southeast;
            black_plus_stock[4] += 13 * orange_southeast;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.OrangeSplitSouthEast, 0, 12, 13, 0, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.OrangeSplitSouthEast, 0, 12 * (orange_item - orange_southeast), 13 * (orange_item - orange_southeast), 0, 0, AdviceType.BonusEnd));
        }

        if (orange_item > 0)
        {
            if (pink_item == 0)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.OrangeColorNoPink, 0, 0, 0, 0, 50, AdviceType.Bonus));
            }

            if (lightblue_item == 0)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.OrangeColorNoLightBlue, 0, 0, 50, 0, 0, AdviceType.Bonus));
            }
        }

        //ベージュ関連
        int beige_cream = 0;
        int beige_cream_northeast = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if ((furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Beige) >= 0)
               || (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Cream) >= 0))
            {
                ++beige_cream;
                if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.NorthWest)
                {
                    ++beige_cream_northeast;
                }

            }
        }

        if (room_direction_ == Direction.NorthWest)
        {
            //北西ベージュクリーム
            plus_luck_[0] += 15 * beige_cream;
            plus_luck_[3] += 10 * beige_cream;

            black_plus_stock[0] += 15 * beige_cream;
            black_plus_stock[3] += 10 * beige_cream;
            comment_flag_.Add(new CommentFlag(CommentIdentifier.BeigeCreamNorthWest, 15, 0, 0, 10, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.BeigeCreamNorthWest, 15 * (limit_furniture_ - beige_cream), 0, 0, 10 * (limit_furniture_ - beige_cream), 0, AdviceType.BonusEnd));
        }
        else
        {
            plus_luck_[0] += 15 * beige_cream_northeast;
            plus_luck_[3] += 10 * beige_cream_northeast;

            black_plus_stock[0] += 15 * beige_cream_northeast;
            black_plus_stock[3] += 10 * beige_cream_northeast;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.BeigeCreamSplitNorthWest, 15, 0, 0, 10, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.BeigeCreamSplitNorthWest, 15 * (beige_cream - beige_cream_northeast), 0, 0, 10 * (beige_cream - beige_cream_northeast), 0, AdviceType.BonusEnd));
        }

        //銀色関連
        plus_luck_[3] += 10 * silver_item;
        black_plus_stock[3] += 10 * silver_item;
        comment_flag_.Add(new CommentFlag(CommentIdentifier.SilverInteger, 0, 0, 0, 10, 0, AdviceType.BonusGame));
        comment_flag_.Add(new CommentFlag(CommentIdentifier.SilverInteger, 0, 0, 0, 10 * (limit_furniture_ - silver_item), 0, AdviceType.BonusEnd));

        //黒の色の引き締め効果
        for (int i = 0; i < 5; ++i)
        {
            black_plus_stock[i] /= 10;
            black_minus_stock[i] /= 10;
        }

        if (black_item > 0)
        {
            for (int i = 0; i < 5; ++i)
            {
                plus_luck_[i] += black_plus_stock[i];
                minus_luck_[i] += black_minus_stock[i];

            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.BlackStrengthening,
                  black_minus_stock[0] - black_plus_stock[0],
                  black_minus_stock[1] - black_plus_stock[1],
                  black_minus_stock[2] - black_plus_stock[2],
                  black_minus_stock[3] - black_plus_stock[3],
                  black_minus_stock[4] - black_plus_stock[4], AdviceType.BonusGame));
        }
        else
        {
            comment_flag_.Add(new CommentFlag(CommentIdentifier.BlackNoStrengthening,
                  -black_minus_stock[0] + black_plus_stock[0],
                  -black_minus_stock[1] + black_plus_stock[1],
                  -black_minus_stock[2] + black_plus_stock[2],
                  -black_minus_stock[3] + black_plus_stock[3],
                  -black_minus_stock[4] + black_plus_stock[4], AdviceType.BonusGame));
        }
    }

    //材質
    private void FortuneMaterial()
    {

    }


    //柄(模様)
    private void FortunePattern()
    {

    }

    //形状
    private void FortuneForm()
    {
        int high_item = 0;
        int low_item = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].form_type().IndexOf(FurnitureGrid.FormType.High) >= 0)
            {
                ++high_item;
            }

            if (furniture_grid_[i].form_type().IndexOf(FurnitureGrid.FormType.Low) >= 0)
            {
                ++low_item;
            }
        }

        //背が高い家具関連
        int high_northeast = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].form_type().IndexOf(FurnitureGrid.FormType.High) >= 0)
            {
                if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.NorthEast)
                {
                    ++high_northeast;
                }
            }
        }

        if (room_direction_ == Direction.NorthEast)
        {
            //北東の部屋は背の高い家具がいいです
            plus_luck_[0] += 5 * high_item;
            plus_luck_[1] += 5 * high_item;
            plus_luck_[2] += 5 * high_item;
            plus_luck_[3] += 5 * high_item;
            plus_luck_[4] += 5 * high_item;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.HighFormNorthEast, 5, 5, 5, 5, 5, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.HighFormNorthEast,
                5 * (limit_high_form_ - high_item),
                5 * (limit_high_form_ - high_item),
                5 * (limit_high_form_ - high_item),
                5 * (limit_high_form_ - high_item),
                5 * (limit_high_form_ - high_item), AdviceType.BonusEnd));
        }
        else
        {

            plus_luck_[0] += 5 * high_northeast;
            plus_luck_[1] += 5 * high_northeast;
            plus_luck_[2] += 5 * high_northeast;
            plus_luck_[3] += 5 * high_northeast;
            plus_luck_[4] += 5 * high_northeast;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.HighFormSplitNorthEast, 5, 5, 5, 5, 5, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.HighFormSplitNorthEast,
                5 * (high_item - high_northeast),
                5 * (high_item - high_northeast),
                5 * (high_item - high_northeast),
                5 * (high_item - high_northeast),
                5 * (high_item - high_northeast), AdviceType.BonusEnd));
        }



        //背が低い家具関連
        int low_southwest = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].form_type().IndexOf(FurnitureGrid.FormType.Low) >= 0)
            {
                if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.SouthWest)
                {
                    ++low_southwest;
                }
            }
        }

        if (room_direction_ == Direction.SouthWest)
        {
            //南西の部屋は背の低い家具がいいです
            plus_luck_[0] += 10 * low_item;
            plus_luck_[1] += 5 * low_item;
            plus_luck_[2] += 10 * low_item;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.LowFormSouthWest, 10, 5, 10, 0, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.LowFormSouthWest, 10 * (limit_furniture_ - low_item), 5 * (limit_furniture_ - low_item), 10 * (limit_furniture_ - low_item), 0, 0, AdviceType.BonusEnd));
        }
        else
        {
            plus_luck_[0] += 10 * low_southwest;
            plus_luck_[1] += 5 * low_southwest;
            plus_luck_[2] += 10 * low_southwest;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.LowFormSplitSouthWest, 10, 5, 10, 0, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.LowFormSplitSouthWest, 10 * (low_item - low_southwest), 5 * (low_item - low_southwest), 10 * (low_item - low_southwest), 0, 0, AdviceType.BonusEnd));
        }
    }


    //その他特性
    private void FortuneCharacteristic()
    {
        int luxury_item = 0;
        int sound_item = 0;
        int smell_item = 0;
        int warm_item = 0;
        int cold_item = 0;
        int wind_item = 0;
        int western_item = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Luxury) >= 0)
            {
                ++luxury_item;
            }

            if (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Sound) >= 0)
            {
                ++sound_item;
            }

            if (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Smell) >= 0)
            {
                ++smell_item;
            }

            if (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Warm) >= 0)
            {
                ++warm_item;
            }

            if (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Cold) >= 0)
            {
                ++cold_item;
            }

            if (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Wind) >= 0)
            {
                ++wind_item;
            }

            if (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Western) >= 0)
            {
                ++western_item;
            }

        }


        //高級家具関連
        int luxury_northwest = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Luxury) >= 0)
            {
                if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.NorthWest)
                {
                    ++luxury_northwest;
                }
            }
        }

        if (room_direction_ == Direction.NorthWest)
        {
            //北西の部屋は高級家具がいいです
            if (luxury_item > 0)
            {
                plus_luck_[0] += 15 * luxury_item;
                plus_luck_[3] += 10 * luxury_item;
                comment_flag_.Add(new CommentFlag(CommentIdentifier.LuxuryNorthWest, 15, 0, 0, 10, 0, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.LuxuryNorthWest, 15 * (limit_furniture_ - luxury_item), 0, 0, 10 * (limit_furniture_ - luxury_item), 0, AdviceType.BonusEnd));
            }
            else
            {
                minus_luck_[0] += 50;
                minus_luck_[1] += 50;
                comment_flag_.Add(new CommentFlag(CommentIdentifier.LuxuryZeroNorthWest, 65, 50, 0, 10, 0, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.LuxuryZeroNorthWest, 15 * (limit_furniture_ - luxury_item) + 50,
                    50, 0, 10 * (limit_furniture_ - luxury_item), 0, AdviceType.BonusEnd));
            }
        }
        else
        {
            plus_luck_[0] += 15 * luxury_northwest;
            plus_luck_[3] += 10 * luxury_northwest;
            comment_flag_.Add(new CommentFlag(CommentIdentifier.LuxurySplitNorthWest, 15, 0, 0, 10, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.LuxurySplitNorthWest, 15 * (luxury_item - luxury_northwest), 0, 0, 10 * (luxury_item - luxury_northwest), 0, AdviceType.BonusEnd));
        }


        //音関連
        //東，南東方位では音がでる，いいにおい，風関連がいい
        int sound_smell_wind = 0;
        int sound_east_southeast = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if (((furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Sound) >= 0)
                || (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Smell) >= 0))
                || (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Wind) >= 0))
            {
                ++sound_smell_wind;
                if ((furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.East)
                    || (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.SouthEast))
                {
                    ++sound_east_southeast;
                }
            }
        }

        if (room_direction_ == Direction.East)
        {
            //東の部屋は音，におい，風家具がいいです
            plus_luck_[1] += 10 * sound_smell_wind;
            plus_luck_[4] += 15 * sound_smell_wind;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.SoundEast, 0, 10, 0, 0, 15, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.SoundEast, 0, 10 * (limit_furniture_ - sound_smell_wind), 0, 0, 15 * (limit_furniture_ - sound_smell_wind), AdviceType.BonusEnd));
        }
        else if (room_direction_ == Direction.SouthEast)
        {
            //南東の部屋は音，におい，風家具がいいです
            plus_luck_[1] += 10 * sound_smell_wind;
            plus_luck_[4] += 15 * sound_smell_wind;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.SoundSouthEast, 0, 10, 0, 0, 15, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.SoundSouthEast, 0, 10 * (limit_furniture_ - sound_smell_wind), 0, 0, 15 * (limit_furniture_ - sound_smell_wind), AdviceType.BonusEnd));
        }
        else
        {
            plus_luck_[1] += 10 * sound_east_southeast;
            plus_luck_[4] += 15 * sound_east_southeast;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.SoundSplitEastSouthEast, 0, 10, 0, 0, 15, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.SoundSplitEastSouthEast, 0, 10 * (sound_smell_wind - sound_east_southeast), 0, 0, 15 * (sound_smell_wind - sound_east_southeast), AdviceType.BonusEnd));
        }

        //温かみ
        plus_luck_[2] += 10 * warm_item;
        comment_flag_.Add(new CommentFlag(CommentIdentifier.WarmInteger, 0, 0, 10, 0, 0, AdviceType.BonusGame));
        comment_flag_.Add(new CommentFlag(CommentIdentifier.WarmInteger, 0, 0, 10 * (limit_furniture_ - warm_item), 0, 0, AdviceType.BonusEnd));

        //冷たさ
        int cold_north = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Cold) >= 0)
            {
                if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.North)
                {
                    ++cold_north;
                }
            }
        }

        if (room_direction_ == Direction.North)
        {
            //北方位に冷たい家具置かないように
            minus_luck_[2] -= 40 * cold_item;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.ColdNorth, 0, 0, 40, 0, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.ColdNorth, 0, 0, 40 * cold_item, 0, 0, AdviceType.BonusEnd));
        }
        else
        {
            plus_luck_[2] -= 20 * cold_north;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.ColdSplitNorth, 0, 0, 20, 0, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.ColdSplitNorth, 0, 0, 20 * cold_north, 0, 0, AdviceType.BonusEnd));
        }

        //西洋風関連
        int western_west = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Western) >= 0)
            {
                if (furniture_grid_[i].placed_direction() == FurnitureGrid.PlacedDirection.West)
                {
                    ++western_west;
                }
            }
        }

        if (room_direction_ == Direction.West)
        {
            //南西の部屋は背の低い家具がいいです
            plus_luck_[3] += 25 * western_item;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.WesternWest, 0, 0, 0, 25, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.WesternWest, 0, 0, 0, 25 * (limit_furniture_ - western_item), 0, AdviceType.BonusEnd));
        }
        else
        {
            plus_luck_[3] += 25 * western_west;

            comment_flag_.Add(new CommentFlag(CommentIdentifier.WesternSplitWest, 0, 0, 0, 25, 0, AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.WesternSplitWest, 0, 0, 0, 25 * (western_item - western_west), 0, AdviceType.BonusEnd));
        }

    }

    //部屋による運勢補正
    private void FortuneRoom()
    {

        if (room_role_ == Room.Bedroom)
        {

        }
        else if (room_role_ == Room.Living)
        {

        }
        else if (room_role_ == Room.Workroom)
        {

        }
    }

    //部屋の方位による運勢補正
    private void FortuneDirection()
    {
        if (room_direction_ == Direction.North)
        {
            //北は水の気でパワーアップ
            int direction_power;
            if (elements_[4] <= limit_elements_)
            {
                direction_power = (energy_strength_ / 5 + elements_[4] - 200) / 2;
                if (direction_power < 0)
                {
                    direction_power = 0;
                }
            }
            else
            {
                direction_power = (energy_strength_ / 5 + limit_elements_ - 200) / 2;
            }

            plus_luck_[3] += direction_power / 2;
            plus_luck_[4] += direction_power / 2;

            if (direction_power < 400)
            {
                //北の部屋のパワー弱すぎて金運，恋愛運が上がらない
                if (elements_[4] < 500)
                {
                    if (direction_power < 336)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWeak, 0, 0, 0, 32, 32, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWeak, 0, 0, 0, (400 - direction_power) / 2, (400 - direction_power) / 2, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWeak, 0, 0, 0, (400 - direction_power) / 2, (400 - direction_power) / 2, AdviceType.ElementEnd));
                }
                else
                {
                    if (direction_power < 336)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWeakOther, 0, 0, 0, 32, 32, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWeakOther, 0, 0, 0, (400 - direction_power) / 2, (400 - direction_power) / 2, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWeakOther, 0, 0, 0, (400 - direction_power) / 2, (400 - direction_power) / 2, AdviceType.ElementEnd));
                }
            }

            //元々健康運，人気運に悪影響
            //温かみのある家具一つにつき北の気の悪影響緩和
            int warm_item = 0;
            for (int i = 0; i < furniture_grid_.Count; ++i)
            {
                //エラー家具を無視する処理
                if (IsIgnored(i))
                {
                    continue;
                }

                if (furniture_grid_[i].characteristic().IndexOf(FurnitureGrid.Characteristic.Warm) >= 0)
                {
                    ++warm_item;
                }
            }

            if (warm_item < 3)
            {
                minus_luck_[1] += 120;
                minus_luck_[2] += 120;

                comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthCold, 0, 40, 40, 0, 0, AdviceType.BonusGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthCold, 0, 40 * (3 - warm_item), 40 * (3 - warm_item), 0, 0, AdviceType.BonusEnd));
            }
        }
        else if (room_direction_ == Direction.NorthEast)
        {
            //北東は木の気でパワーアップ
            int direction_power;
            if (elements_[2] <= limit_elements_)
            {
                direction_power = (energy_strength_ / 5 + elements_[2] - 200) / 2;
                if (direction_power < 0)
                {
                    direction_power = 0;
                }
            }
            else
            {
                direction_power = (energy_strength_ / 5 + limit_elements_ - 200) / 2;
            }

            if ((yin_yang_ >= limit_yin_) && (yin_yang_ <= limit_yang_))
            {
                plus_luck_[0] += direction_power / 5;
                plus_luck_[1] += direction_power / 5;
                plus_luck_[2] += direction_power / 5;
                plus_luck_[3] += direction_power / 5;
                plus_luck_[4] += direction_power / 5;

                if (direction_power < 400)
                {
                    //北東の部屋のパワー弱すぎて全ての運気が上がらない
                    if (elements_[2] < 500)
                    {
                        if (direction_power < 335)
                        {
                            comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthEastWeak, 13, 13, 13, 13, 13, AdviceType.ElementGame));
                        }
                        else
                        {
                            comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthEastWeak,
                               (400 - direction_power) / 5, (400 - direction_power) / 5, (400 - direction_power) / 5, (400 - direction_power) / 5, (400 - direction_power) / 5, AdviceType.ElementGame));
                        }
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthEastWeak,
                            (400 - direction_power) / 5, (400 - direction_power) / 5, (400 - direction_power) / 5, (400 - direction_power) / 5, (400 - direction_power) / 5, AdviceType.ElementEnd));
                    }
                    else
                    {
                        if (direction_power < 335)
                        {
                            comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthEastWeakOther, 13, 13, 13, 13, 13, AdviceType.ElementGame));
                        }
                        else
                        {
                            comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthEastWeakOther,
                               (400 - direction_power) / 5, (400 - direction_power) / 5, (400 - direction_power) / 5, (400 - direction_power) / 5, (400 - direction_power) / 5, AdviceType.ElementGame));
                        }
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthEastWeakOther,
                            (400 - direction_power) / 5, (400 - direction_power) / 5, (400 - direction_power) / 5, (400 - direction_power) / 5, (400 - direction_power) / 5, AdviceType.ElementEnd));
                    }
                }
            }
            else
            {
                minus_luck_[0] += direction_power / 10;
                minus_luck_[1] += direction_power / 10;
                minus_luck_[2] += direction_power / 10;
                minus_luck_[3] += direction_power / 10;
                minus_luck_[4] += direction_power / 10;

                //北東の陰陽バランスが悪いので全ての運気に悪影響
                comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthEastMinus,
                          (400 - direction_power) / 10, (400 - direction_power) / 10, (400 - direction_power) / 10, (400 - direction_power) / 10, (400 - direction_power) / 10, AdviceType.Element));
            }


        }
        else if (room_direction_ == Direction.East)
        {
            //東は木の気でパワーアップ
            int direction_power;
            if (elements_[0] <= limit_elements_)
            {
                direction_power = (energy_strength_ / 5 + elements_[0] - 200) / 2;
                if (direction_power < 0)
                {
                    direction_power = 0;
                }
            }
            else
            {
                direction_power = (energy_strength_ / 5 + limit_elements_ - 200) / 2;
            }

            plus_luck_[0] += direction_power;

            if (direction_power < 400)
            {
                if (elements_[0] < 500)
                {
                    //東の部屋のパワー弱すぎて仕事運が上がらない
                    if (direction_power < 336)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.EastWeak, 64, 0, 0, 0, 0, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.EastWeak, (400 - direction_power), 0, 0, 0, 0, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.EastWeak, (400 - direction_power), 0, 0, 0, 0, AdviceType.ElementEnd));
                }
                else
                {
                    //東の部屋のパワー弱すぎて仕事運が上がらない
                    if (direction_power < 336)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.EastWeakOther, 64, 0, 0, 0, 0, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.EastWeakOther, (400 - direction_power), 0, 0, 0, 0, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.EastWeakOther, (400 - direction_power), 0, 0, 0, 0, AdviceType.ElementEnd));
                }
            }

        }
        else if (room_direction_ == Direction.SouthEast)
        {
            //南東は木の気でパワーアップ
            int direction_power;
            if (elements_[0] <= limit_elements_)
            {
                direction_power = (energy_strength_ / 5 + elements_[0] - 200) / 2;
                if (direction_power < 0)
                {
                    direction_power = 0;
                }
            }
            else
            {
                direction_power = (energy_strength_ / 5 + limit_elements_ - 200) / 2;
            }

            plus_luck_[1] += direction_power * 2 / 5;
            plus_luck_[4] += direction_power * 3 / 5;

            if (direction_power < 400)
            {
                //南東の部屋のパワー弱すぎて人気運，恋愛運が上がらない
                if (elements_[0] < 500)
                {
                    if (direction_power < 335)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthEastWeak, 0, 26, 0, 0, 39, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthEastWeak, 0, (400 - direction_power) * 2 / 5, 0, 0, (400 - direction_power) * 3 / 5, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthEastWeak, 0, (400 - direction_power) * 2 / 5, 0, 0, (400 - direction_power) * 3 / 5, AdviceType.ElementEnd));
                }
                else
                {
                    if (direction_power < 335)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthEastWeakOther, 0, 26, 0, 0, 39, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthEastWeakOther, 0, (400 - direction_power) * 2 / 5, 0, 0, (400 - direction_power) * 3 / 5, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthEastWeakOther, 0, (400 - direction_power) * 2 / 5, 0, 0, (400 - direction_power) * 3 / 5, AdviceType.ElementEnd));
                }
            }
        }
        else if (room_direction_ == Direction.South)
        {
            //南は火の気でパワーアップ
            int direction_power;
            if (elements_[1] <= limit_elements_)
            {
                direction_power = (energy_strength_ / 5 + elements_[1] - 200) / 2;
                if (direction_power < 0)
                {
                    direction_power = 0;
                }
            }
            else
            {
                direction_power = (energy_strength_ / 5 + limit_elements_ - 300) / 2;
            }

            plus_luck_[1] += direction_power * 3 / 5;
            plus_luck_[2] += direction_power / 5;
            plus_luck_[4] += direction_power / 5;

            if (direction_power < 400)
            {
                //南の部屋のパワー弱すぎて人気運，健康運，恋愛運が上がらない
                if (elements_[1] < 500)
                {
                    if (direction_power < 335)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthWeak, 0, 39, 13, 0, 13, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthWeak, 0, (400 - direction_power) * 3 / 5, (400 - direction_power) / 5, 0, (400 - direction_power) / 5, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthWeak, 0, (400 - direction_power) * 3 / 5, (400 - direction_power) / 5, 0, (400 - direction_power) / 5, AdviceType.ElementEnd));
                }
                else
                {
                    if (direction_power < 335)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthWeakOther, 0, 39, 13, 0, 13, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthWeakOther, 0, (400 - direction_power) * 3 / 5, (400 - direction_power) / 5, 0, (400 - direction_power) / 5, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthWeakOther, 0, (400 - direction_power) * 3 / 5, (400 - direction_power) / 5, 0, (400 - direction_power) / 5, AdviceType.ElementEnd));
                }
            }
        }
        else if (room_direction_ == Direction.SouthWest)
        {
            //南西は土の気でパワーアップ
            int direction_power = energy_strength_ / 5;
            if (elements_[2] <= limit_elements_)
            {
                direction_power = (energy_strength_ / 5 + elements_[2] - 200) / 2;
                if (direction_power < 0)
                {
                    direction_power = 0;
                }
            }
            else
            {
                direction_power = (energy_strength_ / 5 + limit_elements_ - 200) / 2;
            }

            plus_luck_[0] += direction_power / 3;
            plus_luck_[1] += direction_power / 3;
            plus_luck_[2] += direction_power / 3;

            if (direction_power < 400)
            {
                if (elements_[2] < 500)
                {
                    //南西の部屋のパワー弱すぎて仕事運，人気運，健康運が上がらない
                    if (direction_power < 336)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthWestWeak, 21, 22, 21, 0, 0, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthWestWeak, (400 - direction_power) / 3, (400 - direction_power) / 3, (400 - direction_power) / 3, 0, 0, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthWestWeak, (400 - direction_power) / 3, (400 - direction_power) / 3, (400 - direction_power) / 3, 0, 0, AdviceType.ElementEnd));
                }
                else
                {
                    //南西の部屋のパワー弱すぎて仕事運，人気運，健康運が上がらない
                    if (direction_power < 336)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthWestWeakOther, 21, 22, 21, 0, 0, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthWestWeakOther, (400 - direction_power) / 3, (400 - direction_power) / 3, (400 - direction_power) / 3, 0, 0, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthWestWeakOther, (400 - direction_power) / 3, (400 - direction_power) / 3, (400 - direction_power) / 3, 0, 0, AdviceType.ElementEnd));
                }
            }
        }
        else if (room_direction_ == Direction.West)
        {
            //西は金の気でパワーアップ
            int direction_power;
            if (elements_[3] <= limit_elements_)
            {
                direction_power = (energy_strength_ / 5 + elements_[3] - 200) / 2;
                if (direction_power < 0)
                {
                    direction_power = 0;
                }
            }
            else
            {
                direction_power = (energy_strength_ / 5 + limit_elements_ - 200) / 2;
            }

            plus_luck_[3] += direction_power / 2;
            plus_luck_[4] += direction_power / 2;

            if (direction_power < 400)
            {
                if (elements_[3] < 500)
                {
                    //西の部屋のパワー弱すぎて金運，恋愛運が上がらない
                    if (direction_power < 336)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.WestWeak, 0, 0, 0, 32, 32, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.WestWeak, 0, 0, 0, (400 - direction_power) / 2, (400 - direction_power) / 2, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.WestWeak, 0, 0, 0, (400 - direction_power) / 2, (400 - direction_power) / 2, AdviceType.ElementEnd));
                }
                else
                {
                    //西の部屋のパワー弱すぎて金運，恋愛運が上がらない
                    if (direction_power < 336)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.WestWeakOther, 0, 0, 0, 32, 32, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.WestWeakOther, 0, 0, 0, (400 - direction_power) / 2, (400 - direction_power) / 2, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.WestWeakOther, 0, 0, 0, (400 - direction_power) / 2, (400 - direction_power) / 2, AdviceType.ElementEnd));
                }
            }
        }
        else if (room_direction_ == Direction.NorthWest)
        {
            //北西は金の気でパワーアップ
            int direction_power;
            if (elements_[3] <= limit_elements_)
            {
                direction_power = (energy_strength_ / 5 + elements_[3] - 200) / 2;
                if (direction_power < 0)
                {
                    direction_power = 0;
                }
            }
            else
            {
                direction_power = (energy_strength_ / 5 + limit_elements_ - 200) / 2;
            }

            plus_luck_[0] += direction_power / 2;
            plus_luck_[3] += direction_power / 2;

            if (direction_power < 400)
            {
                //北西の部屋のパワー弱すぎて仕事運，金運が上がらない
                if (elements_[3] < 500)
                {
                    if (direction_power < 336)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWestWeak, 32, 0, 0, 32, 0, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWestWeak, (400 - direction_power) / 2, 0, 0, (400 - direction_power) / 2, 0, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWestWeak, (400 - direction_power) / 2, 0, 0, (400 - direction_power) / 2, 0, AdviceType.ElementEnd));
                }
                else
                {
                    if (direction_power < 336)
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWestWeakOther, 32, 0, 0, 32, 0, AdviceType.ElementGame));
                    }
                    else
                    {
                        comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWestWeakOther, (400 - direction_power) / 2, 0, 0, (400 - direction_power) / 2, 0, AdviceType.ElementGame));
                    }
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWestWeakOther, (400 - direction_power) / 2, 0, 0, (400 - direction_power) / 2, 0, AdviceType.ElementEnd));
                }
            }

            //北西の金の気が強すぎると仕事運，人気運下がる
            if (elements_[3] > limit_elements_)
            {
                minus_luck_[0] = elements_[3] - limit_elements_;
                minus_luck_[1] = elements_[3] - limit_elements_;

                comment_flag_.Add(new CommentFlag(CommentIdentifier.NorthWestVain, (400 - direction_power) / 2, (400 - direction_power) / 2, 0, 0, 0, AdviceType.Element));
            }
        }
    }

    //部屋の小方位による運勢補正(部屋の中の方位)
    private void FortuneSplitDirection()
    {
        int direction_power;

        //北は水の気でパワーアップ
        if (split_elements_[4][0] <= 125)
        {
            direction_power = split_elements_[4][0] - 25;
            if (direction_power < 0)
            {
                direction_power = 0;
            }
        }
        else
        {
            direction_power = 100;
        }

        plus_luck_[3] += direction_power / 2;
        plus_luck_[4] += direction_power / 2;

        if (direction_power < 100)
        {
            //北の部屋のパワー弱すぎて金運，恋愛運が上がらない
            if (elements_[4] - direction_power + 100 > limit_elements_)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitNorthWeak, 0, 0, 0, (limit_elements_ - elements_[4]) / 2, (limit_elements_ - elements_[4]) / 2, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitNorthWeak, 0, 0, 0, (100 - direction_power) / 2, (100 - direction_power) / 2, AdviceType.ElementGame));
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitNorthWeak, 0, 0, 0, (100 - direction_power) / 2, (100 - direction_power) / 2, AdviceType.ElementEnd));
        }

        //北東は土の気でパワーアップ
        if (split_elements_[2][1] <= 125)
        {
            direction_power = split_elements_[2][1] - 25;
            if (direction_power < 0)
            {
                direction_power = 0;
            }
        }
        else
        {
            direction_power = 100;
        }

        if (split_yin_yang_[1] >= -125 && split_yin_yang_[1] <= 125)
        {
            plus_luck_[0] += direction_power / 5;
            plus_luck_[1] += direction_power / 5;
            plus_luck_[2] += direction_power / 5;
            plus_luck_[3] += direction_power / 5;
            plus_luck_[4] += direction_power / 5;

            if (direction_power < 125)
            {
                //北東の部屋のパワー弱すぎて全ての運気が上がらない
                if (elements_[2] - direction_power + 100 > limit_elements_)
                {
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitNorthEastWeak,
                        (limit_elements_ - elements_[2]) / 5, (limit_elements_ - elements_[2]) / 5, (limit_elements_ - elements_[2]) / 5, (limit_elements_ - elements_[2]) / 5, (limit_elements_ - elements_[2]) / 5, AdviceType.ElementGame));
                }
                else
                {
                    comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitNorthEastWeak,
                       (100 - direction_power) / 5, (100 - direction_power) / 5, (100 - direction_power) / 5, (100 - direction_power) / 5, (100 - direction_power) / 5, AdviceType.ElementGame));
                }
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitNorthEastWeak,
                        (100 - direction_power) / 5, (100 - direction_power) / 5, (100 - direction_power) / 5, (100 - direction_power) / 5, (100 - direction_power) / 5, AdviceType.ElementEnd));
            }
        }
        else
        {
            minus_luck_[0] += direction_power / 10;
            minus_luck_[1] += direction_power / 10;
            minus_luck_[2] += direction_power / 10;
            minus_luck_[3] += direction_power / 10;
            minus_luck_[4] += direction_power / 10;

            //北東の陰陽バランスが悪いので全ての運気に悪影響
            comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitNorthEastMinus,
                         (100 - direction_power) / 10, (100 - direction_power) / 10, (100 - direction_power) / 10, (100 - direction_power) / 10, (100 - direction_power) / 10, AdviceType.Element));
        }

        //東は木の気でパワーアップ
        if (split_elements_[0][2] <= 125)
        {
            direction_power = split_elements_[0][2] - 25;
            if (direction_power < 0)
            {
                direction_power = 0;
            }
        }
        else
        {
            direction_power = 100;
        }

        plus_luck_[0] += direction_power;

        if (direction_power < 100)
        {
            //東の部屋のパワー弱すぎて仕事運が上がらない
            if (elements_[0] - direction_power + 100 > limit_elements_)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitEastWeak, (limit_elements_ - elements_[0]), 0, 0, 0, 0, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitEastWeak, (100 - direction_power), 0, 0, 0, 0, AdviceType.ElementGame));
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitEastWeak, (100 - direction_power), 0, 0, 0, 0, AdviceType.ElementEnd));
        }

        //南東は木の気でパワーアップ
        if (split_elements_[0][3] <= 125)
        {
            direction_power = split_elements_[0][3] - 25;
            if (direction_power < 0)
            {
                direction_power = 0;
            }
        }
        else
        {
            direction_power = 100;
        }

        plus_luck_[1] += direction_power * 2 / 5;
        plus_luck_[4] += direction_power * 3 / 5;

        if (direction_power < 100)
        {
            //南東の部屋のパワー弱すぎて人気運，恋愛運が上がらない
            if (elements_[0] - direction_power + 100 > limit_elements_)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitSouthEastWeak, 0, (limit_elements_ - elements_[0]) * 2 / 5, 0, 0, (limit_elements_ - elements_[0]) * 3 / 5, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitSouthEastWeak, 0, (100 - direction_power) * 2 / 5, 0, 0, (100 - direction_power) * 3 / 5, AdviceType.ElementGame));
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitSouthEastWeak, 0, (100 - direction_power) * 2 / 5, 0, 0, (100 - direction_power) * 3 / 5, AdviceType.ElementEnd));
        }

        //南は火の気でパワーアップ
        if (split_elements_[1][4] <= 125)
        {
            direction_power = split_elements_[1][4] - 25;
            if (direction_power < 0)
            {
                direction_power = 0;
            }
        }
        else
        {
            direction_power = 100;
        }

        plus_luck_[1] += direction_power * 3 / 5;
        plus_luck_[2] += direction_power / 5;
        plus_luck_[4] += direction_power / 5;

        if (direction_power < 100)
        {
            //南の部屋のパワー弱すぎて人気運，健康運，恋愛運が上がらない
            if (elements_[1] - direction_power + 100 > limit_elements_)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitSouthWeak, 0, (limit_elements_ - elements_[1]) * 3 / 5, (limit_elements_ - elements_[1]) / 5, 0, (limit_elements_ - elements_[1]) / 5, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitSouthWeak, 0, (100 - direction_power) * 3 / 5, (100 - direction_power) / 5, 0, (100 - direction_power) / 5, AdviceType.ElementGame));
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitSouthWeak, 0, (100 - direction_power) * 3 / 5, (100 - direction_power) / 5, 0, (100 - direction_power) / 5, AdviceType.ElementEnd));
        }

        //南西は土の気でパワーアップ
        if (split_elements_[2][5] <= 125)
        {
            direction_power = split_elements_[2][5] - 25;
            if (direction_power < 0)
            {
                direction_power = 0;
            }
        }
        else
        {
            direction_power = 100;
        }

        plus_luck_[0] += direction_power / 3;
        plus_luck_[1] += direction_power / 3;
        plus_luck_[2] += direction_power / 3;

        if (direction_power < 100)
        {
            //南西の部屋のパワー弱すぎて仕事運，人気運，健康運が上がらない
            if (elements_[2] - direction_power + 100 > limit_elements_)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitSouthWestWeak, (limit_elements_ - elements_[2]) / 3, (limit_elements_ - elements_[2]) / 3, (limit_elements_ - elements_[2]) / 3, 0, 0, AdviceType.ElementGame));
            }

            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitSouthWestWeak, (100 - direction_power) / 3, (100 - direction_power) / 3, (100 - direction_power) / 3, 0, 0, AdviceType.ElementGame));
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitSouthWestWeak, (100 - direction_power) / 3, (100 - direction_power) / 3, (100 - direction_power) / 3, 0, 0, AdviceType.ElementEnd));
        }

        //西は金の気でパワーアップ
        if (split_elements_[3][6] <= 125)
        {
            direction_power = split_elements_[3][6] - 25;
            if (direction_power < 0)
            {
                direction_power = 0;
            }
        }
        else
        {
            direction_power = 100;
        }

        plus_luck_[3] += direction_power / 2;
        plus_luck_[4] += direction_power / 2;

        if (direction_power < 100)
        {
            //西の部屋のパワー弱すぎて金運，恋愛運が上がらない
            if (elements_[3] - direction_power + 100 > limit_elements_)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitWestWeak, 0, 0, 0, (limit_elements_ - elements_[3]) / 2, (limit_elements_ - elements_[3]) / 2, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitWestWeak, 0, 0, 0, (100 - direction_power) / 2, (100 - direction_power) / 2, AdviceType.ElementGame));
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitWestWeak, 0, 0, 0, (100 - direction_power) / 2, (100 - direction_power) / 2, AdviceType.ElementEnd));
        }

        //北西は金の気でパワーアップ
        if (split_elements_[3][7] <= 125)
        {
            direction_power = split_elements_[3][7] - 25;
            if (direction_power < 0)
            {
                direction_power = 0;
            }
        }
        else
        {
            direction_power = 100;
        }

        plus_luck_[0] += direction_power / 2;
        plus_luck_[3] += direction_power / 2;

        if (direction_power < 100)
        {
            //北西の部屋のパワー弱すぎて仕事運，金運が上がらない
            if (elements_[3] - direction_power + 100 > limit_elements_)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitNorthWestWeak, (limit_elements_ - elements_[3]) / 2, 0, 0, (limit_elements_ - elements_[3]) / 2, 0, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitNorthWestWeak, (100 - direction_power) / 2, 0, 0, (100 - direction_power) / 2, 0, AdviceType.ElementGame));
            }

            comment_flag_.Add(new CommentFlag(CommentIdentifier.SplitNorthWestWeak, (100 - direction_power) / 2, 0, 0, (100 - direction_power) / 2, 0, AdviceType.ElementEnd));
        }

    }

    //五行による補正
    private void FortuneFiveElement()
    {
        //木の気の影響
        if (elements_[0] <= limit_elements_)
        {
            plus_luck_[0] += elements_[0] * 4 / 5;
            plus_luck_[2] += elements_[0] / 5;
        }
        else
        {
            plus_luck_[0] += limit_elements_ * 4 / 5;
            plus_luck_[2] += limit_elements_ / 5;

            //木の気が強すぎて仕事運、悪影響
            int wood_over = (elements_[0] - limit_elements_);

            minus_luck_[0] += wood_over;

            if (wood_over > 100)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodOver, 100, 0, 0, 0, 0, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodOver, wood_over, 0, 0, 0, 0, AdviceType.ElementGame));
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodOver, wood_over, 0, 0, 0, 0, AdviceType.ElementEnd));
        }

        //火の気の影響
        if (elements_[1] <= limit_elements_)
        {
            plus_luck_[1] += elements_[1] * 3 / 5;
            plus_luck_[2] += elements_[1] / 5;
            plus_luck_[4] += elements_[1] / 5;
        }
        else
        {
            plus_luck_[1] += limit_elements_ * 3 / 5;
            plus_luck_[2] += limit_elements_ / 5;
            plus_luck_[4] += limit_elements_ / 5;

            //火の気が強すぎて仕事運，健康運，恋愛運に悪影響
            int fire_over = (elements_[1] - limit_elements_);

            minus_luck_[0] += fire_over / 3;
            minus_luck_[2] += fire_over / 3;
            minus_luck_[4] += fire_over / 3;

            if (fire_over > 100)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FireOver, 0, 34, 33, 0, 33, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FireOver, 0, fire_over / 3, fire_over / 3, 0, fire_over / 3, AdviceType.ElementGame));
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.FireOver, 0, fire_over / 3, fire_over / 3, 0, fire_over / 3, AdviceType.ElementEnd));
        }

        //土の気の影響
        if (elements_[2] <= limit_elements_)
        {
            plus_luck_[0] += elements_[0] / 5;
            plus_luck_[1] += elements_[1] / 5;
            plus_luck_[2] += elements_[2] / 5;
            plus_luck_[3] += elements_[3] / 5;
            plus_luck_[4] += elements_[4] / 5;

        }
        else
        {
            plus_luck_[0] += limit_elements_ / 5;
            plus_luck_[1] += limit_elements_ / 5;
            plus_luck_[2] += limit_elements_ / 5;
            plus_luck_[3] += limit_elements_ / 5;
            plus_luck_[4] += limit_elements_ / 5;

            //土の気が強すぎて健康運に悪影響
            int earth_over = (elements_[2] - limit_elements_);

            minus_luck_[2] += earth_over;

            if (earth_over > 100)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthOver, 0, 0, 100, 0, 0, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthOver, 0, 0, earth_over, 0, 0, AdviceType.ElementGame));
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthOver, 0, 0, earth_over, 0, 0, AdviceType.ElementEnd));
        }

        //金の気の影響
        if (elements_[3] <= limit_elements_)
        {
            plus_luck_[3] += elements_[3];

        }
        else
        {
            plus_luck_[3] += limit_elements_;

            //金の気が強すぎて金運に悪影響
            int metal_over = (elements_[3] - limit_elements_);

            minus_luck_[3] += metal_over;

            if (metal_over > 100)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalOver, 0, 0, 0, 100, 0, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalOver, 0, 0, 0, metal_over, 0, AdviceType.ElementGame));
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalOver, 0, 0, 0, metal_over, 0, AdviceType.ElementEnd));
        }

        //水の気の影響
        if (elements_[4] <= limit_elements_)
        {
            plus_luck_[0] += elements_[4] / 5;
            plus_luck_[3] += elements_[4] / 5;
            plus_luck_[4] += elements_[4] * 3 / 5;
        }
        else
        {
            plus_luck_[0] += limit_elements_ / 5;
            plus_luck_[3] += limit_elements_ / 5;
            plus_luck_[4] += limit_elements_ * 3 / 5;

            //水の気が強すぎて健康運，金運, 恋愛運に悪影響
            int water_over = (elements_[4] - limit_elements_);
            minus_luck_[2] += water_over / 5;
            minus_luck_[3] += water_over * 2 / 5;
            minus_luck_[4] += water_over * 2 / 5;
            if (water_over > 100)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterOver, 0, 0, 20, 40, 40, AdviceType.ElementGame));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterOver, 0, 0, water_over / 5, water_over * 2 / 5, water_over * 2 / 5, AdviceType.ElementGame));
            }

            comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterOver, 0, 0, water_over / 5, water_over * 2 / 5, water_over * 2 / 5, AdviceType.ElementEnd));
        }

        //相生で消された木(ゲーム中)
        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSoshoNorth,
           sosho_buffer_[0][0] * 4 / 5, -sosho_buffer_[0][0] * 6 / 5, (sosho_buffer_[0][0] / 5 - sosho_buffer_[0][0] * 2 / 5), 0, -sosho_buffer_[0][0] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSoshoNorthEast,
          sosho_buffer_[0][1] * 4 / 5, -sosho_buffer_[0][1] * 6 / 5, (sosho_buffer_[0][1] / 5 - sosho_buffer_[0][1] * 2 / 5), 0, -sosho_buffer_[0][1] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSoshoEast,
         sosho_buffer_[0][2] * 4 / 5, -sosho_buffer_[0][2] * 6 / 5, (sosho_buffer_[0][2] / 5 - sosho_buffer_[0][2] * 2 / 5), 0, -sosho_buffer_[0][2] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSoshoSouthEast,
          sosho_buffer_[0][3] * 4 / 5, -sosho_buffer_[0][3] * 6 / 5, (sosho_buffer_[0][3] / 5 - sosho_buffer_[0][3] * 2 / 5), 0, -sosho_buffer_[0][3] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSoshoSouth,
          sosho_buffer_[0][4] * 4 / 5, -sosho_buffer_[0][4] * 6 / 5, (sosho_buffer_[0][4] / 5 - sosho_buffer_[0][4] * 2 / 5), 0, -sosho_buffer_[0][4] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSoshoSouthWest,
         sosho_buffer_[0][5] * 4 / 5, -sosho_buffer_[0][5] * 6 / 5, (sosho_buffer_[0][5] / 5 - sosho_buffer_[0][5] * 2 / 5), 0, -sosho_buffer_[0][5] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSoshoWest,
         sosho_buffer_[0][6] * 4 / 5, -sosho_buffer_[0][6] * 6 / 5, (sosho_buffer_[0][6] / 5 - sosho_buffer_[0][6] * 2 / 5), 0, -sosho_buffer_[0][6] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSoshoNorthWest,
         sosho_buffer_[0][7] * 4 / 5, -sosho_buffer_[0][7] * 6 / 5, (sosho_buffer_[0][7] / 5 - sosho_buffer_[0][7] * 2 / 5), 0, -sosho_buffer_[0][7] * 2 / 5, AdviceType.ElementGame));


        //相生で消された火(ゲーム中)
        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSoshoNorth, -sosho_buffer_[1][0] * 2 / 5, (sosho_buffer_[1][0] * 3 / 5 - sosho_buffer_[1][0] * 2 / 5),
            (sosho_buffer_[1][0] / 5 - sosho_buffer_[1][0] * 2 / 5), -sosho_buffer_[1][0] * 2 / 5, (sosho_buffer_[1][0] / 5 - sosho_buffer_[1][0] * 2 / 5), AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSoshoNorthEast, -sosho_buffer_[1][1] * 2 / 5, (sosho_buffer_[1][1] * 3 / 5 - sosho_buffer_[1][1] * 2 / 5),
          (sosho_buffer_[1][1] / 5 - sosho_buffer_[1][1] * 2 / 5), -sosho_buffer_[1][1] * 2 / 5, (sosho_buffer_[1][1] / 5 - sosho_buffer_[1][1] * 2 / 5), AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSoshoEast, -sosho_buffer_[1][2] * 2 / 5, (sosho_buffer_[1][2] * 3 / 5 - sosho_buffer_[1][2] * 2 / 5),
           (sosho_buffer_[1][2] / 5 - sosho_buffer_[1][2] * 2 / 5), -sosho_buffer_[1][2] * 2 / 5, (sosho_buffer_[1][2] / 5 - sosho_buffer_[1][2] * 2 / 5), AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSoshoSouthEast, -sosho_buffer_[1][3] * 2 / 5, (sosho_buffer_[1][3] * 3 / 5 - sosho_buffer_[1][3] * 2 / 5),
          (sosho_buffer_[1][3] / 5 - sosho_buffer_[1][3] * 2 / 5), -sosho_buffer_[1][3] * 2 / 5, (sosho_buffer_[1][3] / 5 - sosho_buffer_[1][3] * 2 / 5), AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSoshoSouth, -sosho_buffer_[1][4] * 2 / 5, (sosho_buffer_[1][4] * 3 / 5 - sosho_buffer_[1][4] * 2 / 5),
           (sosho_buffer_[1][4] / 5 - sosho_buffer_[1][4] * 2 / 5), -sosho_buffer_[1][4] * 2 / 5, (sosho_buffer_[1][4] / 5 - sosho_buffer_[1][4] * 2 / 5), AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSoshoSouthWest, -sosho_buffer_[1][5] * 2 / 5, (sosho_buffer_[1][5] * 3 / 5 - sosho_buffer_[1][5] * 2 / 5),
          (sosho_buffer_[1][5] / 5 - sosho_buffer_[1][5] * 2 / 5), -sosho_buffer_[1][5] * 2 / 5, (sosho_buffer_[1][5] / 5 - sosho_buffer_[1][5] * 2 / 5), AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSoshoWest, -sosho_buffer_[1][6] * 2 / 5, (sosho_buffer_[1][6] * 3 / 5 - sosho_buffer_[1][6] * 2 / 5),
           (sosho_buffer_[1][6] / 5 - sosho_buffer_[1][6] * 2 / 5), -sosho_buffer_[1][6] * 2 / 5, (sosho_buffer_[1][6] / 5 - sosho_buffer_[1][6] * 2 / 5), AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSoshoNorthWest, -sosho_buffer_[1][7] * 2 / 5, (sosho_buffer_[1][7] * 3 / 5 - sosho_buffer_[1][7] * 2 / 5),
          (sosho_buffer_[1][7] / 5 - sosho_buffer_[1][7] * 2 / 5), -sosho_buffer_[1][7] * 2 / 5, (sosho_buffer_[1][7] / 5 - sosho_buffer_[1][7] * 2 / 5), AdviceType.ElementGame));


        //相生で消された土(ゲーム中)
        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSoshoNorth,
            sosho_buffer_[2][0] / 5, sosho_buffer_[2][0] / 5, sosho_buffer_[2][0] / 5, (sosho_buffer_[2][0] / 5 - sosho_buffer_[2][0] * 2), sosho_buffer_[2][0] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSoshoNorthEast,
           sosho_buffer_[2][1] / 5, sosho_buffer_[2][1] / 5, sosho_buffer_[2][1] / 5, (sosho_buffer_[2][1] / 5 - sosho_buffer_[2][1] * 2), sosho_buffer_[2][1] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSoshoEast,
           sosho_buffer_[2][2] / 5, sosho_buffer_[2][2] / 5, sosho_buffer_[2][2] / 5, (sosho_buffer_[2][2] / 5 - sosho_buffer_[2][2] * 2), sosho_buffer_[2][2] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSoshoSouthEast,
           sosho_buffer_[2][3] / 5, sosho_buffer_[2][3] / 5, sosho_buffer_[2][3] / 5, (sosho_buffer_[2][3] / 5 - sosho_buffer_[2][3] * 2), sosho_buffer_[2][3] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSoshoSouth,
           sosho_buffer_[2][4] / 5, sosho_buffer_[2][4] / 5, sosho_buffer_[2][4] / 5, (sosho_buffer_[2][4] / 5 - sosho_buffer_[2][4] * 2), sosho_buffer_[2][4] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSoshoSouthWest,
          sosho_buffer_[2][5] / 5, sosho_buffer_[2][5] / 5, sosho_buffer_[2][5] / 5, (sosho_buffer_[2][5] / 5 - sosho_buffer_[2][5] * 2), sosho_buffer_[2][5] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSoshoWest,
           sosho_buffer_[2][6] / 5, sosho_buffer_[2][6] / 5, sosho_buffer_[2][6] / 5, (sosho_buffer_[2][6] / 5 - sosho_buffer_[2][6] * 2), sosho_buffer_[2][6] * 2 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSoshoNorthWest,
           sosho_buffer_[2][7] / 5, sosho_buffer_[2][7] / 5, sosho_buffer_[2][7] / 5, (sosho_buffer_[2][7] / 5 - sosho_buffer_[2][7] * 2), sosho_buffer_[2][7] * 2 / 5, AdviceType.ElementGame));


        //相生で消された金(ゲーム中)
        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSoshoNorth,
            -sosho_buffer_[3][0] * 2 / 5, 0, 0, (sosho_buffer_[3][0] - sosho_buffer_[3][0] * 2 / 5), -sosho_buffer_[3][0] * 6 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSoshoNorthEast,
          -sosho_buffer_[3][1] * 2 / 5, 0, 0, (sosho_buffer_[3][1] - sosho_buffer_[3][1] * 2 / 5), -sosho_buffer_[3][1] * 6 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSoshoEast,
          -sosho_buffer_[3][2] * 2 / 5, 0, 0, (sosho_buffer_[3][2] - sosho_buffer_[3][2] * 2 / 5), -sosho_buffer_[3][2] * 6 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSoshoSouthEast,
          -sosho_buffer_[3][3] * 2 / 5, 0, 0, (sosho_buffer_[3][3] - sosho_buffer_[3][3] * 2 / 5), -sosho_buffer_[3][3] * 6 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSoshoSouth,
          -sosho_buffer_[3][4] * 2 / 5, 0, 0, (sosho_buffer_[3][4] - sosho_buffer_[3][4] * 2 / 5), -sosho_buffer_[3][4] * 6 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSoshoSouthWest,
          -sosho_buffer_[3][5] * 2 / 5, 0, 0, (sosho_buffer_[3][5] - sosho_buffer_[3][5] * 2 / 5), -sosho_buffer_[3][5] * 6 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSoshoWest,
        -sosho_buffer_[3][6] * 2 / 5, 0, 0, (sosho_buffer_[3][6] - sosho_buffer_[3][6] * 2 / 5), -sosho_buffer_[3][6] * 6 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSoshoNorthWest,
          -sosho_buffer_[3][7] * 2 / 5, 0, 0, (sosho_buffer_[3][7] - sosho_buffer_[3][7] * 2 / 5), -sosho_buffer_[3][7] * 6 / 5, AdviceType.ElementGame));


        //相生で消された水(ゲーム中)
        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSoshoNorth,
            (sosho_buffer_[4][0] / 5 - sosho_buffer_[4][0] * 8 / 5), 0, -sosho_buffer_[4][0] * 2 / 5, sosho_buffer_[4][0] / 5, sosho_buffer_[4][0] * 3 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSoshoNorthEast,
            (sosho_buffer_[4][1] / 5 - sosho_buffer_[4][1] * 8 / 5), 0, -sosho_buffer_[4][1] * 2 / 5, sosho_buffer_[4][1] / 5, sosho_buffer_[4][1] * 3 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSoshoEast,
           (sosho_buffer_[4][2] / 5 - sosho_buffer_[4][2] * 8 / 5), 0, -sosho_buffer_[4][2] * 2 / 5, sosho_buffer_[4][2] / 5, sosho_buffer_[4][2] * 3 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSoshoSouthEast,
            (sosho_buffer_[4][3] / 5 - sosho_buffer_[4][3] * 8 / 5), 0, -sosho_buffer_[4][3] * 2 / 5, sosho_buffer_[4][3] / 5, sosho_buffer_[4][3] * 3 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSoshoSouth,
           (sosho_buffer_[4][4] / 5 - sosho_buffer_[4][4] * 8 / 5), 0, -sosho_buffer_[4][4] * 2 / 5, sosho_buffer_[4][4] / 5, sosho_buffer_[4][4] * 3 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSoshoSouthWest,
            (sosho_buffer_[4][5] / 5 - sosho_buffer_[4][5] * 8 / 5), 0, -sosho_buffer_[4][5] * 2 / 5, sosho_buffer_[4][5] / 5, sosho_buffer_[4][5] * 3 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSoshoWest,
           (sosho_buffer_[4][6] / 5 - sosho_buffer_[4][6] * 8 / 5), 0, -sosho_buffer_[4][6] * 2 / 5, sosho_buffer_[4][6] / 5, sosho_buffer_[4][6] * 3 / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSoshoNorthWest,
            (sosho_buffer_[4][7] / 5 - sosho_buffer_[4][7] * 8 / 5), 0, -sosho_buffer_[4][7] * 2 / 5, sosho_buffer_[4][7] / 5, sosho_buffer_[4][7] * 3 / 5, AdviceType.ElementGame));


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        //相克で消された気(木)
        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSokokuNorth,
          sokoku_buffer_[0][0] * 4 / 5, 0, sokoku_buffer_[0][0] / 5, 0, 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSokokuNorthEast,
         sokoku_buffer_[0][1] * 4 / 5, 0, sokoku_buffer_[0][1] / 5, 0, 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSokokuEast,
         sokoku_buffer_[0][2] * 4 / 5, 0, sokoku_buffer_[0][2] / 5, 0, 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSokokuSouthEast,
         sokoku_buffer_[0][3] * 4 / 5, 0, sokoku_buffer_[0][3] / 5, 0, 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSokokuSouth,
         sokoku_buffer_[0][4] * 4 / 5, 0, sokoku_buffer_[0][4] / 5, 0, 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSokokuSouthWest,
         sokoku_buffer_[0][5] * 4 / 5, 0, sokoku_buffer_[0][5] / 5, 0, 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSokokuWest,
         sokoku_buffer_[0][6] * 4 / 5, 0, sokoku_buffer_[0][6] / 5, 0, 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSokokuNorthWest,
         sokoku_buffer_[0][7] * 4 / 5, 0, sokoku_buffer_[0][7] / 5, 0, 0, AdviceType.ElementGame));


        //相克で消された気(火)
        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSokokuNorth,
          0, sokoku_buffer_[1][0] * 3 / 5, sokoku_buffer_[1][0] / 5, 0, sokoku_buffer_[1][0] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSokokuNorthEast,
           0, sokoku_buffer_[1][1] * 3 / 5, sokoku_buffer_[1][1] / 5, 0, sokoku_buffer_[1][1] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSokokuEast,
        0, sokoku_buffer_[1][2] * 3 / 5, sokoku_buffer_[1][2] / 5, 0, sokoku_buffer_[1][2] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSokokuSouthEast,
           0, sokoku_buffer_[1][3] * 3 / 5, sokoku_buffer_[1][3] / 5, 0, sokoku_buffer_[1][3] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSokokuSouth,
       0, sokoku_buffer_[1][4] * 3 / 5, sokoku_buffer_[1][4] / 5, 0, sokoku_buffer_[1][4] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSokokuSouthWest,
           0, sokoku_buffer_[1][5] * 3 / 5, sokoku_buffer_[1][5] / 5, 0, sokoku_buffer_[1][5] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSokokuWest,
        0, sokoku_buffer_[1][6] * 3 / 5, sokoku_buffer_[1][6] / 5, 0, sokoku_buffer_[1][6] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSokokuNorthWest,
           0, sokoku_buffer_[1][7] * 3 / 5, sokoku_buffer_[1][7] / 5, 0, sokoku_buffer_[1][7] / 5, AdviceType.ElementGame));


        //相克で消された気(土)
        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSokokuNorth,
          sokoku_buffer_[2][0] / 5, sokoku_buffer_[2][0] / 5, sokoku_buffer_[2][0] / 5, sokoku_buffer_[2][0] / 5, sokoku_buffer_[2][0] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSokokuNorthEast,
           sokoku_buffer_[2][1] / 5, sokoku_buffer_[2][1] / 5, sokoku_buffer_[2][1] / 5, sokoku_buffer_[2][1] / 5, sokoku_buffer_[2][1] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSokokuEast,
         sokoku_buffer_[2][2] / 5, sokoku_buffer_[2][2] / 5, sokoku_buffer_[2][2] / 5, sokoku_buffer_[2][2] / 5, sokoku_buffer_[2][2] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSokokuSouthEast,
           sokoku_buffer_[2][3] / 5, sokoku_buffer_[2][3] / 5, sokoku_buffer_[2][3] / 5, sokoku_buffer_[2][3] / 5, sokoku_buffer_[2][3] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSokokuSouth,
         sokoku_buffer_[2][4] / 5, sokoku_buffer_[2][4] / 5, sokoku_buffer_[2][4] / 5, sokoku_buffer_[2][4] / 5, sokoku_buffer_[2][4] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSokokuSouthWest,
           sokoku_buffer_[2][5] / 5, sokoku_buffer_[2][5] / 5, sokoku_buffer_[2][5] / 5, sokoku_buffer_[2][5] / 5, sokoku_buffer_[2][5] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSokokuWest,
         sokoku_buffer_[2][6] / 5, sokoku_buffer_[2][6] / 5, sokoku_buffer_[2][6] / 5, sokoku_buffer_[2][6] / 5, sokoku_buffer_[2][6] / 5, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSokokuNorthWest,
           sokoku_buffer_[2][7] / 5, sokoku_buffer_[2][7] / 5, sokoku_buffer_[2][7] / 5, sokoku_buffer_[2][7] / 5, sokoku_buffer_[2][7] / 5, AdviceType.ElementGame));


        //相克で消された気(金)
        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSokokuNorth, 0, 0, 0, sokoku_buffer_[3][0], 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSokokuNorthEast, 0, 0, 0, sokoku_buffer_[3][1], 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSokokuEast, 0, 0, 0, sokoku_buffer_[3][2], 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSokokuSouthEast, 0, 0, 0, sokoku_buffer_[3][3], 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSokokuSouth, 0, 0, 0, sokoku_buffer_[3][4], 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSokokuSouthWest, 0, 0, 0, sokoku_buffer_[3][5], 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSokokuWest, 0, 0, 0, sokoku_buffer_[3][6], 0, AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSokokuNorthWest, 0, 0, 0, sokoku_buffer_[3][7], 0, AdviceType.ElementGame));


        //相克で消された気(水)
        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSokokuNorth,
          sokoku_buffer_[4][0] / 5, 0, 0, sokoku_buffer_[4][0] / 5, sokoku_buffer_[4][0], AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSokokuNorthEast,
         sokoku_buffer_[4][1] / 5, 0, 0, sokoku_buffer_[4][1] / 5, sokoku_buffer_[4][1], AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSokokuEast,
         sokoku_buffer_[4][2] / 5, 0, 0, sokoku_buffer_[4][2] / 5, sokoku_buffer_[4][2], AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSokokuSouthEast,
         sokoku_buffer_[4][3] / 5, 0, 0, sokoku_buffer_[4][3] / 5, sokoku_buffer_[4][3], AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSokokuSouth,
         sokoku_buffer_[4][4] / 5, 0, 0, sokoku_buffer_[4][4] / 5, sokoku_buffer_[4][4], AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSokokuSouthWest,
         sokoku_buffer_[4][5] / 5, 0, 0, sokoku_buffer_[4][5] / 5, sokoku_buffer_[4][5], AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSokokuWest,
         sokoku_buffer_[4][6] / 5, 0, 0, sokoku_buffer_[4][6] / 5, sokoku_buffer_[4][6], AdviceType.ElementGame));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSokokuNorthWest,
         sokoku_buffer_[4][7] / 5, 0, 0, sokoku_buffer_[4][7] / 5, sokoku_buffer_[4][7], AdviceType.ElementGame));


        //相乗で消された気(最終評価)
        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSosho,
            sokoku_buffer_[0].Sum() * 4 / 5, -sokoku_buffer_[0].Sum() * 6 / 5, (sokoku_buffer_[0].Sum() / 5 - sokoku_buffer_[0].Sum() * 2 / 5), 0, -sokoku_buffer_[0].Sum() * 2 / 5, AdviceType.ElementEnd));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSosho, -sokoku_buffer_[1].Sum() * 2 / 5, (sokoku_buffer_[1].Sum() * 3 / 5 - sokoku_buffer_[1].Sum() * 2 / 5),
         (sokoku_buffer_[1].Sum() / 5 - sokoku_buffer_[1].Sum() * 2 / 5), -sokoku_buffer_[1].Sum() * 2 / 5, (sokoku_buffer_[1].Sum() / 5 - sokoku_buffer_[1].Sum() * 2 / 5), AdviceType.ElementEnd));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSosho,
         sokoku_buffer_[2].Sum() / 5, sokoku_buffer_[2].Sum() / 5, sokoku_buffer_[2].Sum() / 5, (sokoku_buffer_[2].Sum() / 5 - sokoku_buffer_[2].Sum() * 2), sokoku_buffer_[2].Sum() * 2 / 5, AdviceType.ElementEnd));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSosho,
                   -sokoku_buffer_[3].Sum() * 2 / 5, 0, 0, (sokoku_buffer_[3].Sum() - sokoku_buffer_[3].Sum() * 2 / 5), -sokoku_buffer_[3].Sum() * 6 / 5, AdviceType.ElementEnd));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSosho,
        (sokoku_buffer_[4].Sum() / 5 - sokoku_buffer_[4].Sum() * 8 / 5), 0, -sokoku_buffer_[4].Sum() * 2 / 5, sokoku_buffer_[4].Sum() / 5, sokoku_buffer_[4].Sum() * 3 / 5, AdviceType.ElementEnd));


        //相克で消された気(最終評価)
        comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodSokoku,
        sokoku_buffer_[0].Sum() * 4 / 5, 0, sokoku_buffer_[0].Sum() / 5, 0, 0, AdviceType.ElementEnd));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.FireSokoku,
        0, sokoku_buffer_[1].Sum() * 3 / 5, sokoku_buffer_[1].Sum() / 5, 0, sokoku_buffer_[1].Sum() / 5, AdviceType.ElementEnd));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthSokoku,
      sokoku_buffer_[2].Sum() / 5, sokoku_buffer_[2].Sum() / 5, sokoku_buffer_[2].Sum() / 5, sokoku_buffer_[2].Sum() / 5, sokoku_buffer_[2].Sum() / 5, AdviceType.ElementEnd));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalSokoku, 0, 0, 0, sokoku_buffer_[3].Sum(), 0, AdviceType.ElementEnd));

        comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterSokoku,
         sokoku_buffer_[4].Sum() / 5, 0, 0, sokoku_buffer_[4].Sum() / 5, sokoku_buffer_[4].Sum(), AdviceType.ElementEnd));

        //部屋の気を上げましょう-----------------------------------------------------------------------------------------------------------------------------------------------------------

        int[][][] expected_elements = new int[5][][];

        //k = 評価したい五行，i = 各方位の五行, j = 方位
        for (int k = 0; k < 5; ++k)
        {
            expected_elements[k] = new int[5][];
            for (int i = 0; i < 5; ++i)
            {
                expected_elements[k][i] = new int[8] { split_elements_[i][0], split_elements_[i][1], split_elements_[i][2], split_elements_[i][3],
               split_elements_[i][4], split_elements_[i][5], split_elements_[i][6], split_elements_[i][7], };
                for (int j = 0; j < 5; ++j)
                {
                    expected_elements[k][i][j] -= (sosho_buffer_[i][j] + sokoku_buffer_[i][j]);
                    if (k == i)
                    {
                        expected_elements[k][i][j] += 100;
                    }
                }
            }

            //ここから相生の処理
            int[][] sosho_stock = new int[5][];
            for (int i = 0; i < 5; ++i)
            {
                sosho_stock[i] = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            }

            for (int j = 0; j < 8; ++j)
            {
                for (int i = 0; i < 5; ++i)
                {
                    //同じ方位
                    if (expected_elements[k][i][j] / 2 <= expected_elements[k][(i + 1) % 5][j])
                    {
                        sosho_stock[(i + 1) % 5][j] += expected_elements[k][i][j] / 2;
                        sosho_stock[i][j] -= expected_elements[k][i][j] / 4;
                    }
                    else
                    {
                        sosho_stock[(i + 1) % 5][j] += expected_elements[k][(i + 1) % 5][j];
                        sosho_stock[i][j] -= expected_elements[k][(i + 1) % 5][j] / 2;
                    }

                    //時計隣
                    if (expected_elements[k][i][j] / 4 <= expected_elements[k][(i + 1) % 5][(j + 1) % 8])
                    {
                        sosho_stock[(i + 1) % 5][(j + 1) % 8] += expected_elements[k][i][j] / 4;
                        sosho_stock[i][j] -= expected_elements[k][i][j] / 8;
                    }
                    else
                    {
                        sosho_stock[(i + 1) % 5][(j + 1) % 8] += expected_elements[k][(i + 1) % 5][(j + 1) % 8];
                        sosho_stock[i][j] -= expected_elements[k][(i + 1) % 5][(j + 1) % 8] / 2;
                    }

                    //反時計隣
                    if (expected_elements[k][i][j] / 4 <= expected_elements[k][(i + 1) % 5][(j + 7) % 8])
                    {
                        sosho_stock[(i + 1) % 5][(j + 7) % 8] += expected_elements[k][i][j] / 4;
                        sosho_stock[i][j] -= expected_elements[k][i][j] / 8;
                    }
                    else
                    {
                        sosho_stock[(i + 1) % 5][(j + 7) % 8] += expected_elements[k][(i + 1) % 5][(j + 7) % 8];
                        sosho_stock[i][j] -= expected_elements[k][(i + 1) % 5][(j + 7) % 8] / 2;
                    }
                }
            }

            for (int j = 0; j < 8; ++j)
            {
                for (int i = 0; i < 5; ++i)
                {
                    if ((expected_elements[k][i][j] + sosho_stock[i][j]) < 0)
                    {
                        expected_elements[k][i][j] = 0;
                    }
                    else
                    {
                        expected_elements[k][i][j] += sosho_stock[i][j];
                    }
                }
            }

            //ここから相克の処理
            int[][] sokoku_stock = new int[5][];
            for (int i = 0; i < 5; ++i)
            {
                sokoku_stock[i] = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            }

            for (int j = 0; j < 8; ++j)
            {
                for (int i = 0; i < 5; ++i)
                {

                    //同じ方位
                    if (expected_elements[k][i][j] / 2 <= expected_elements[k][(i + 2) % 5][j])
                    {
                        sokoku_stock[(i + 2) % 5][j] -= expected_elements[k][i][j] / 2;
                        sokoku_stock[i][j] -= expected_elements[k][i][j] / 4;
                    }
                    else
                    {
                        sokoku_stock[(i + 2) % 5][j] -= expected_elements[k][(i + 2) % 5][j];
                        sokoku_stock[i][j] -= expected_elements[k][(i + 2) % 5][j] / 2;
                    }

                    //時計隣
                    if (expected_elements[k][i][j] / 4 <= expected_elements[k][(i + 2) % 5][(j + 1) % 8])
                    {
                        sokoku_stock[(i + 2) % 5][(j + 1) % 8] -= expected_elements[k][i][j] / 4;
                        sokoku_stock[i][j] -= expected_elements[k][i][j] / 8;
                    }
                    else
                    {
                        sokoku_stock[(i + 2) % 5][(j + 1) % 8] -= expected_elements[k][(i + 2) % 5][(j + 1) % 8];
                        sokoku_stock[i][j] -= expected_elements[k][(i + 2) % 5][(j + 1) % 8] / 2;
                    }

                    //反時計隣
                    if (expected_elements[k][i][j] / 4 <= expected_elements[k][(i + 2) % 5][(j + 7) % 8])
                    {
                        sokoku_stock[(i + 2) % 5][(j + 7) % 8] -= expected_elements[k][i][j] / 4;
                        sokoku_stock[i][j] -= expected_elements[k][i][j] / 8;
                    }
                    else
                    {
                        sokoku_stock[(i + 2) % 5][(j + 7) % 8] -= expected_elements[k][(i + 2) % 5][(j + 7) % 8];
                        sokoku_stock[i][j] -= expected_elements[k][(i + 2) % 5][(j + 7) % 8] / 2;
                    }

                }

            }


            for (int j = 0; j < 8; ++j)
            {
                for (int i = 0; i < 5; ++i)
                {
                    if ((expected_elements[k][i][j] + sokoku_stock[i][j]) < 0)
                    {
                        expected_elements[k][i][j] = 0;
                    }
                    else
                    {
                        expected_elements[k][i][j] += sokoku_stock[i][j];
                    }
                }
            }

            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    expected_elements[k][i][j] -= split_elements_[i][j];
                }
            }

            int[][] expected_luck = new int[5][];
            for (int i = 0; i < 5; ++i)
            {
                expected_luck[i] = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            }

            for (int j = 0; j < 8; ++j)
            {
                expected_luck[0][j] += expected_elements[k][0][j] * 4 / 5 + expected_elements[k][2][j] / 5 + expected_elements[k][4][j] / 5;
                expected_luck[1][j] += expected_elements[k][1][j] * 3 / 5 + expected_elements[k][2][j] / 5;
                expected_luck[2][j] += expected_elements[k][0][j] / 5 + expected_elements[k][1][j] / 5 + expected_elements[k][2][j] / 5;
                expected_luck[3][j] += expected_elements[k][2][j] / 5 + expected_elements[k][3][j] + expected_elements[k][4][j] / 5;
                expected_luck[4][j] += expected_elements[k][1][j] / 5 + expected_elements[k][2][j] / 5 + expected_elements[k][4][j] * 3 / 5;
            }

            if (k == 0)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodWeakNorth, expected_luck[0][0], expected_luck[1][0], expected_luck[2][0], expected_luck[3][0], expected_luck[4][0], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodWeakNorthEast, expected_luck[0][1], expected_luck[1][1], expected_luck[2][1], expected_luck[3][1], expected_luck[4][1], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodWeakEast, expected_luck[0][2], expected_luck[1][2], expected_luck[2][2], expected_luck[3][2], expected_luck[4][2], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodWeakSouthEast, expected_luck[0][3], expected_luck[1][3], expected_luck[2][3], expected_luck[3][3], expected_luck[4][3], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodWeakSouth, expected_luck[0][4], expected_luck[1][4], expected_luck[2][4], expected_luck[3][4], expected_luck[4][4], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodWeakSouthWest, expected_luck[0][5], expected_luck[1][5], expected_luck[2][5], expected_luck[3][5], expected_luck[4][5], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodWeakWest, expected_luck[0][6], expected_luck[1][6], expected_luck[2][6], expected_luck[3][6], expected_luck[4][6], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodWeakNorthWest, expected_luck[0][7], expected_luck[1][7], expected_luck[2][7], expected_luck[3][7], expected_luck[4][7], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WoodWeak, expected_luck[0].Sum(), expected_luck[1].Sum(), expected_luck[2].Sum(), expected_luck[3].Sum(), expected_luck[4].Sum(), AdviceType.ElementEnd));
            }
            else if (k == 1)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FireWeakNorth, expected_luck[0][0], expected_luck[1][0], expected_luck[2][0], expected_luck[3][0], expected_luck[4][0], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FireWeakNorthEast, expected_luck[0][1], expected_luck[1][1], expected_luck[2][1], expected_luck[3][1], expected_luck[4][1], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FireWeakEast, expected_luck[0][2], expected_luck[1][2], expected_luck[2][2], expected_luck[3][2], expected_luck[4][2], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FireWeakSouthEast, expected_luck[0][3], expected_luck[1][3], expected_luck[2][3], expected_luck[3][3], expected_luck[4][3], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FireWeakSouth, expected_luck[0][4], expected_luck[1][4], expected_luck[2][4], expected_luck[3][4], expected_luck[4][4], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FireWeakSouthWest, expected_luck[0][5], expected_luck[1][5], expected_luck[2][5], expected_luck[3][5], expected_luck[4][5], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FireWeakWest, expected_luck[0][6], expected_luck[1][6], expected_luck[2][6], expected_luck[3][6], expected_luck[4][6], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FireWeakNorthWest, expected_luck[0][7], expected_luck[1][7], expected_luck[2][7], expected_luck[3][7], expected_luck[4][7], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.FireWeak, expected_luck[0].Sum(), expected_luck[1].Sum(), expected_luck[2].Sum(), expected_luck[3].Sum(), expected_luck[4].Sum(), AdviceType.ElementEnd));
            }
            else if (k == 2)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthWeakNorth, expected_luck[0][0], expected_luck[1][0], expected_luck[2][0], expected_luck[3][0], expected_luck[4][0], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthWeakNorthEast, expected_luck[0][1], expected_luck[1][1], expected_luck[2][1], expected_luck[3][1], expected_luck[4][1], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthWeakEast, expected_luck[0][2], expected_luck[1][2], expected_luck[2][2], expected_luck[3][2], expected_luck[4][2], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthWeakSouthEast, expected_luck[0][3], expected_luck[1][3], expected_luck[2][3], expected_luck[3][3], expected_luck[4][3], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthWeakSouth, expected_luck[0][4], expected_luck[1][4], expected_luck[2][4], expected_luck[3][4], expected_luck[4][4], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthWeakSouthWest, expected_luck[0][5], expected_luck[1][5], expected_luck[2][5], expected_luck[3][5], expected_luck[4][5], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthWeakWest, expected_luck[0][6], expected_luck[1][6], expected_luck[2][6], expected_luck[3][6], expected_luck[4][6], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthWeakNorthWest, expected_luck[0][7], expected_luck[1][7], expected_luck[2][7], expected_luck[3][7], expected_luck[4][7], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.EarthWeak, expected_luck[0].Sum(), expected_luck[1].Sum(), expected_luck[2].Sum(), expected_luck[3].Sum(), expected_luck[4].Sum(), AdviceType.ElementEnd));
            }
            else if (k == 3)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalWeakNorth, expected_luck[0][0], expected_luck[1][0], expected_luck[2][0], expected_luck[3][0], expected_luck[4][0], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalWeakNorthEast, expected_luck[0][1], expected_luck[1][1], expected_luck[2][1], expected_luck[3][1], expected_luck[4][1], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalWeakEast, expected_luck[0][2], expected_luck[1][2], expected_luck[2][2], expected_luck[3][2], expected_luck[4][2], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalWeakSouthEast, expected_luck[0][3], expected_luck[1][3], expected_luck[2][3], expected_luck[3][3], expected_luck[4][3], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalWeakSouth, expected_luck[0][4], expected_luck[1][4], expected_luck[2][4], expected_luck[3][4], expected_luck[4][4], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalWeakSouthWest, expected_luck[0][5], expected_luck[1][5], expected_luck[2][5], expected_luck[3][5], expected_luck[4][5], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalWeakWest, expected_luck[0][6], expected_luck[1][6], expected_luck[2][6], expected_luck[3][6], expected_luck[4][6], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalWeakNorthWest, expected_luck[0][7], expected_luck[1][7], expected_luck[2][7], expected_luck[3][7], expected_luck[4][7], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.MetalWeak, expected_luck[0].Sum(), expected_luck[1].Sum(), expected_luck[2].Sum(), expected_luck[3].Sum(), expected_luck[4].Sum(), AdviceType.ElementEnd));
            }
            else
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterWeakNorth, expected_luck[0][0], expected_luck[1][0], expected_luck[2][0], expected_luck[3][0], expected_luck[4][0], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterWeakNorthEast, expected_luck[0][1], expected_luck[1][1], expected_luck[2][1], expected_luck[3][1], expected_luck[4][1], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterWeakEast, expected_luck[0][2], expected_luck[1][2], expected_luck[2][2], expected_luck[3][2], expected_luck[4][2], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterWeakSouthEast, expected_luck[0][3], expected_luck[1][3], expected_luck[2][3], expected_luck[3][3], expected_luck[4][3], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterWeakSouth, expected_luck[0][4], expected_luck[1][4], expected_luck[2][4], expected_luck[3][4], expected_luck[4][4], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterWeakSouthWest, expected_luck[0][5], expected_luck[1][5], expected_luck[2][5], expected_luck[3][5], expected_luck[4][5], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterWeakWest, expected_luck[0][6], expected_luck[1][6], expected_luck[2][6], expected_luck[3][6], expected_luck[4][6], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterWeakNorthWest, expected_luck[0][7], expected_luck[1][7], expected_luck[2][7], expected_luck[3][7], expected_luck[4][7], AdviceType.ElementGame));
                comment_flag_.Add(new CommentFlag(CommentIdentifier.WaterWeak, expected_luck[0].Sum(), expected_luck[1].Sum(), expected_luck[2].Sum(), expected_luck[3].Sum(), expected_luck[4].Sum(), AdviceType.ElementEnd));
            }
        }

    }


    //仕上げの運勢補正(鏡による運勢増減など)
    private void FortuneLast()
    {
        int[] plus_luck_stock = new int[5] { 0, 0, 0, 0, 0 };

        int[] minus_luck_stock = new int[5] { 0, 0, 0, 0, 0 };


        //観葉植物関連
        int foliage_item = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if ((furniture_grid_[i].furniture_type() == FurnitureGrid.FurnitureType.FoliagePlant)
                || (furniture_grid_[i].material_type().IndexOf(FurnitureGrid.MaterialType.ArtificialFoliage) < 0))
            {
                ++foliage_item;
            }

        }

        int[] expected_foliage = new int[5] { 0, 0, 0, 0, 0 };
        int[] expected_foliage_end = new int[5] { 0, 0, 0, 0, 0 };
        for (int i = 0; i < 5; ++i)
        {
            if (foliage_item <= limit_furniture_)
            {
                minus_luck_stock[i] += (int)(minus_luck_[i] * System.Math.Pow((9 / 10), foliage_item) - minus_luck_[i]);
                expected_foliage[i] += (int)(minus_luck_[i] * System.Math.Pow((9 / 10), foliage_item)) - (int)(minus_luck_[i] * System.Math.Pow((9 / 10), (foliage_item + 1)));
                expected_foliage_end[i] += (int)(minus_luck_[i] * System.Math.Pow((9 / 10), foliage_item)) - (int)(minus_luck_[i] * System.Math.Pow((9 / 10), limit_foliage_));
            }

        }
        if (foliage_item < limit_furniture_)
        {
            comment_flag_.Add(new CommentFlag(CommentIdentifier.FoliagePurification, expected_foliage[0], expected_foliage[1], expected_foliage[2], expected_foliage[3], expected_foliage[4], AdviceType.BonusGame));
            comment_flag_.Add(new CommentFlag(CommentIdentifier.FoliagePurification, expected_foliage_end[0], expected_foliage_end[1], expected_foliage_end[2], expected_foliage_end[3], expected_foliage_end[4], AdviceType.BonusEnd));
        }


        //色関連
        int white_item = 0;
        int yellow_brown_ocher = 0;
        int gold_item = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.White) >= 0)
            {
                ++white_item;
            }

            if (((furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Yellow) >= 0)
                || (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Brown) >= 0))
                 || (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Ocher) >= 0)
                )
            {
                ++yellow_brown_ocher;
            }

            if (furniture_grid_[i].color_name().IndexOf(FurnitureGrid.ColorName.Gold) >= 0)
            {
                ++gold_item;
            }

        }

        //白
        if (white_item > 0)
        {
            for (int i = 0; i < 5; ++i)
            {
                minus_luck_stock[i] += (int)(minus_luck_[i] * 4 / 5 - minus_luck_[i]);
            }

        }
        else
        {
            int[] displacement = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < 5; ++i)
            {
                displacement[i] -= (int)(minus_luck_[i] * 4 / 5 - minus_luck_[i]);
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.WhiteColorPurification, displacement[0], displacement[1], displacement[2], displacement[3], displacement[4], AdviceType.Bonus));

        }

        //黄色，茶色，黄土色
        if (yellow_brown_ocher > 0)
        {
            for (int i = 0; i < 5; ++i)
            {
                plus_luck_stock[i] += (int)(plus_luck_[i] * 6 / 5 - plus_luck_[i]);
            }

        }
        else
        {
            int[] displacement = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < 5; ++i)
            {
                displacement[i] += (int)(plus_luck_[i] * 6 / 5 - plus_luck_[i]);
            }
            comment_flag_.Add(new CommentFlag(CommentIdentifier.YellowBrownOcherOne, displacement[0], displacement[1], displacement[2], displacement[3], displacement[4], AdviceType.Bonus));

        }

        //金色関連
        if (gold_item > 0)
        {
            int[] displacement = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < 5; ++i)
            {
                if (i == 3)
                {
                    plus_luck_stock[i] += (int)(plus_luck_[i] * 13 / 10 - plus_luck_[i]);
                    displacement[i] -= (int)(plus_luck_[i] * 13 / 10 - plus_luck_[i]);
                    minus_luck_stock[i] += (int)(minus_luck_[i] * 13 / 10 - minus_luck_[i]);
                    displacement[i] += (int)(minus_luck_[i] * 13 / 10 - minus_luck_[i]);
                }
                else
                {
                    plus_luck_stock[i] += (int)(plus_luck_[i] * 11 / 10 - plus_luck_[i]);
                    displacement[i] -= (int)(plus_luck_[i] * 11 / 10 - plus_luck_[i]);
                    minus_luck_stock[i] += (int)(minus_luck_[i] * 11 / 10 - minus_luck_[i]);
                    displacement[i] += (int)(minus_luck_[i] * 11 / 10 - minus_luck_[i]);
                }
            }

            comment_flag_.Add(new CommentFlag(CommentIdentifier.GoldBad, displacement[0], displacement[1], displacement[2], displacement[3], displacement[4], AdviceType.Bonus));
        }
        else
        {
            int[] displacement = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < 5; ++i)
            {
                if (i == 3)
                {
                    plus_luck_stock[i] += (int)(plus_luck_[i] * 13 / 10 - plus_luck_[i]);
                    displacement[i] += (int)(plus_luck_[i] * 13 / 10 - plus_luck_[i]);
                    minus_luck_stock[i] += (int)(minus_luck_[i] * 13 / 10 - minus_luck_[i]);
                    displacement[i] -= (int)(minus_luck_[i] * 13 / 10 - minus_luck_[i]);
                }
                else
                {
                    plus_luck_stock[i] += (int)(plus_luck_[i] * 11 / 10 - plus_luck_[i]);
                    displacement[i] += (int)(plus_luck_[i] * 11 / 10 - plus_luck_[i]);
                    minus_luck_stock[i] += (int)(minus_luck_[i] * 11 / 10 - minus_luck_[i]);
                    displacement[i] -= (int)(minus_luck_[i] * 11 / 10 - minus_luck_[i]);
                }
            }

            comment_flag_.Add(new CommentFlag(CommentIdentifier.GoldOne, displacement[0], displacement[1], displacement[2], displacement[3], displacement[4], AdviceType.Bonus));

        }



        //形状関連
        int round_item = 0;
        int square_item = 0;
        int sharp_item = 0;
        for (int i = 0; i < furniture_grid_.Count; ++i)
        {
            if (IsIgnored(i))
            {
                continue;
            }

            if ((furniture_grid_[i].form_type().IndexOf(FurnitureGrid.FormType.Ellipse) >= 0)
                || (furniture_grid_[i].form_type().IndexOf(FurnitureGrid.FormType.Round) >= 0))
            {
                ++round_item;
            }

            if ((furniture_grid_[i].form_type().IndexOf(FurnitureGrid.FormType.Square) >= 0)
                 || (furniture_grid_[i].form_type().IndexOf(FurnitureGrid.FormType.Rectangle) >= 0))
            {
                ++round_item;
            }

            if (furniture_grid_[i].form_type().IndexOf(FurnitureGrid.FormType.Sharp) >= 0)

            {
                ++sharp_item;
            }

        }


        //丸関連
        if (round_item > 0)
        {
            int[] displacement = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < 5; ++i)
            {
                plus_luck_stock[i] += (int)(plus_luck_[i] * 9 / 10 - plus_luck_[i]);
                displacement[i] -= (int)(plus_luck_[i] * 9 / 10 - plus_luck_[i]);
                minus_luck_stock[i] += (int)(minus_luck_[i] * 9 / 10 - minus_luck_[i]);
                displacement[i] += (int)(minus_luck_[i] * 9 / 10 - minus_luck_[i]);

            }

            comment_flag_.Add(new CommentFlag(CommentIdentifier.RoundBad, displacement[0], displacement[1], displacement[2], displacement[3], displacement[4], AdviceType.Bonus));
        }
        else
        {
            int[] displacement = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < 5; ++i)
            {

                plus_luck_stock[i] += (int)(plus_luck_[i] * 9 / 10 - plus_luck_[i]);
                displacement[i] += (int)(plus_luck_[i] * 9 / 10 - plus_luck_[i]);
                minus_luck_stock[i] += (int)(minus_luck_[i] * 9 / 10 - minus_luck_[i]);
                displacement[i] -= (int)(minus_luck_[i] * 9 / 10 - minus_luck_[i]);

            }

            comment_flag_.Add(new CommentFlag(CommentIdentifier.RoundOne, displacement[0], displacement[1], displacement[2], displacement[3], displacement[4], AdviceType.Bonus));

        }



        //四角関連
        if (square_item > 0)
        {
            int[] displacement = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < 5; ++i)
            {
                plus_luck_stock[i] += (int)(plus_luck_[i] * 11 / 10 - plus_luck_[i]);
                displacement[i] -= (int)(plus_luck_[i] * 11 / 10 - plus_luck_[i]);
                minus_luck_stock[i] += (int)(minus_luck_[i] * 11 / 10 - minus_luck_[i]);
                displacement[i] += (int)(minus_luck_[i] * 11 / 10 - minus_luck_[i]);

            }

            comment_flag_.Add(new CommentFlag(CommentIdentifier.SquareBad, displacement[0], displacement[1], displacement[2], displacement[3], displacement[4], AdviceType.Bonus));
        }
        else
        {
            int[] displacement = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < 5; ++i)
            {

                plus_luck_stock[i] += (int)(plus_luck_[i] * 11 / 10 - plus_luck_[i]);
                displacement[i] += (int)(plus_luck_[i] * 11 / 10 - plus_luck_[i]);
                minus_luck_stock[i] += (int)(minus_luck_[i] * 11 / 10 - minus_luck_[i]);
                displacement[i] -= (int)(minus_luck_[i] * 11 / 10 - minus_luck_[i]);

            }

            comment_flag_.Add(new CommentFlag(CommentIdentifier.SquareOne, displacement[0], displacement[1], displacement[2], displacement[3], displacement[4], AdviceType.Bonus));

        }

        //尖った
        if (sharp_item > 0)
        {
            int[] displacement = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < 5; ++i)
            {
                minus_luck_stock[i] += (int)(minus_luck_[i] * 21 / 20 - minus_luck_[i]);
                displacement[i] += (int)(minus_luck_[i] * 21 / 20 - minus_luck_[i]);
            }

            comment_flag_.Add(new CommentFlag(CommentIdentifier.SharpBad, displacement[0], displacement[1], displacement[2], displacement[3], displacement[4], AdviceType.Bonus));
        }

        //部屋の種類関連
        if (room_role_ == Room.Bedroom)
        {
            //寝室でプラスの運気，邪気共に1.2倍
            int[] displacement = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < 5; ++i)
            {
                plus_luck_stock[i] += (int)(plus_luck_[i] * 6 / 5 - plus_luck_[i]);
                minus_luck_stock[i] += (int)(minus_luck_[i] * 6 / 5 - minus_luck_[i]);
                displacement[i] = (int)(minus_luck_[i] * 6 / 5 - minus_luck_[i]) - (int)(plus_luck_[i] * 6 / 5 - plus_luck_[i]);
            }

            //寝室の邪気が高く，悪い運気を取り込みやすくなっている．
            comment_flag_.Add(new CommentFlag(CommentIdentifier.BedroomMulti, displacement[0], displacement[1], displacement[2], displacement[3], displacement[4], AdviceType.BonusEnd));

        }
        else if (room_role_ == Room.Living)
        {
            int[] displacement = new int[3] { 0, 0, 0 };
            //リビングで家庭運(仕事運, 人気運, 健康運)の良い運気，悪い運気共に1.2倍
            for (int i = 0; i < 3; ++i)
            {
                plus_luck_stock[i] += (int)(plus_luck_[i] * 6 / 5 - plus_luck_[i]);
                minus_luck_stock[i] += (int)(minus_luck_[i] * 6 / 5 - minus_luck_[i]);
                displacement[i] = (int)(minus_luck_[i] * 6 / 5 - minus_luck_[i]) - (int)(plus_luck_[i] * 6 / 5 - plus_luck_[i]);
            }

            //リビングの邪気が高く，悪い仕事運，人気運，健康運を取り込みやすくなっている．
            comment_flag_.Add(new CommentFlag(CommentIdentifier.LivingMulti, displacement[0], displacement[1], displacement[2], 0, 0, AdviceType.BonusEnd));

        }
        else if (room_role_ == Room.Workroom)
        {
            int displacement = 0;
            //仕事部屋で仕事運のプラスの運気，邪気共に1.2倍
            plus_luck_stock[0] += (int)(plus_luck_[0] * 6 / 5 - plus_luck_[0]);
            minus_luck_stock[0] += (int)(minus_luck_[0] * 6 / 5 - minus_luck_[0]);
            displacement = (int)(minus_luck_[0] * 6 / 5 - minus_luck_[0]) - (int)(plus_luck_[0] * 6 / 5 - plus_luck_[0]);

            //仕事部屋の邪気が高く，悪い運気を取り込みやすくなっている．
            comment_flag_.Add(new CommentFlag(CommentIdentifier.WorkroomMulti, displacement, 0, 0, 0, 0, AdviceType.BonusEnd));
        }


        //部屋の方角関連
        if (room_direction_ == Direction.North)
        {

        }
        else if (room_direction_ == Direction.NorthEast)
        {

        }
        else if (room_direction_ == Direction.East)
        {

        }
        else if (room_direction_ == Direction.SouthEast)
        {

        }
        else if (room_direction_ == Direction.South)
        {

            int[] displacement = new int[5] { 0, 0, 0, 0, 0 };
            //邪気(悪い運気)0.8倍
            for (int i = 0; i < 5; ++i)
            {
                if (energy_strength_ > 2000)
                {
                    minus_luck_stock[i] += (int)(minus_luck_[i] * 4 / 5 - minus_luck_[i]);
                }
                else
                {
                    displacement[i] -= (int)(minus_luck_[i] * 4 / 5 - minus_luck_[i]);
                }
            }

            if (elements_[1] <= 2000)
            {
                comment_flag_.Add(new CommentFlag(CommentIdentifier.SouthPurification, displacement[0], displacement[1], displacement[2], displacement[3], displacement[4], AdviceType.Element));
            }

        }
        else if (room_direction_ == Direction.SouthWest)
        {

        }
        else if (room_direction_ == Direction.West)
        {

        }
        else if (room_direction_ == Direction.NorthWest)
        {

        }

        //最後に元の運勢に補正を加算
        for (int i = 0; i < 5; ++i)
        {
            plus_luck_[i] += plus_luck_stock[i];
            minus_luck_[i] += minus_luck_stock[i];
        }
    }

    //**********************************************************************************************************************************************************************************************

    //家具を無視するかどうか
    private bool IsIgnored(int index)
    {
        for (int i = 0; i < ignore_index_.Count; ++i)
        {
            if (index == ignore_index_[i])
            {
                return true;
            }
        }
        return false;
    }

    partial void EvaluationTotaTestDummy();
    partial void CommentMini();
    partial void Comment();
}