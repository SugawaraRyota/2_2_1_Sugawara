using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//ゲーム途中のワンポイントアドバイス
public partial class Evaluation : MonoBehaviour
{

    partial void CommentMini()
    {
        int comment_num_elements = 1; //五行陰陽関係コメント数
        int comment_num_bonus = 1; //ボーナス点関係コメント数

        //アドバイスのモードによりコメントの重み変更．
        if (advaice_mode_ == 0)
        {
            //仕事運重視
            for (int i = 0; i < comment_flag_.Count; ++i)
            {
                comment_flag_[i].comment_weight_ += comment_flag_[i].work_weight_;
            }
        }
        else if (advaice_mode_ == 1)
        {
            //人気運重視
            //デフォルト
            for (int i = 0; i < comment_flag_.Count; ++i)
            {
                comment_flag_[i].comment_weight_ += comment_flag_[i].popular_weight_;
            }

        }
        else if (advaice_mode_ == 2)
        {
            //健康運重視
            //デフォルト
            for (int i = 0; i < comment_flag_.Count; ++i)
            {
                comment_flag_[i].comment_weight_ += comment_flag_[i].health_weight_;
            }
        }
        else if (advaice_mode_ == 3)
        {
            //金運重視
            //デフォルト
            for (int i = 0; i < comment_flag_.Count; ++i)
            {
                comment_flag_[i].comment_weight_ += comment_flag_[i].economic_weight_;
            }
        }
        else if (advaice_mode_ == 4)
        {
            //恋愛運重視
            //デフォルト
            for (int i = 0; i < comment_flag_.Count; ++i)
            {
                comment_flag_[i].comment_weight_ += comment_flag_[i].love_weight_;
            }
        }
        else
        {
            //デフォルト
            for (int i = 0; i < comment_flag_.Count; ++i)
            {
                //仕事運による重みづけ
                if ((norma_luck_[0] - luck_[0]) > comment_flag_[i].work_weight_)
                {
                    comment_flag_[i].comment_weight_ += comment_flag_[i].work_weight_;
                }
                else if ((norma_luck_[0] - luck_[0]) > 0)
                {
                    comment_flag_[i].comment_weight_ += norma_luck_[0] - luck_[0];
                }

                //人気運による重みづけ
                if ((norma_luck_[1] - luck_[1]) > comment_flag_[i].popular_weight_)
                {
                    comment_flag_[i].comment_weight_ += comment_flag_[i].popular_weight_;
                }
                else if ((norma_luck_[1] - luck_[1]) > 0)
                {
                    comment_flag_[i].comment_weight_ += norma_luck_[1] - luck_[1];
                }

                //健康運による重みづけ
                if ((norma_luck_[2] - luck_[2]) > comment_flag_[i].health_weight_)
                {
                    comment_flag_[i].comment_weight_ += comment_flag_[i].health_weight_;
                }
                else if ((norma_luck_[2] - luck_[2]) > 0)
                {
                    comment_flag_[i].comment_weight_ += norma_luck_[2] - luck_[2];
                }

                //金運による重みづけ
                if ((norma_luck_[3] - luck_[3]) > comment_flag_[i].economic_weight_)
                {
                    comment_flag_[i].comment_weight_ += comment_flag_[i].economic_weight_;
                }
                else if ((norma_luck_[3] - luck_[3]) > 0)
                {
                    comment_flag_[i].comment_weight_ += norma_luck_[3] - luck_[3];
                }

                //恋愛運による重みづけ
                if ((norma_luck_[4] - luck_[4]) > comment_flag_[i].love_weight_)
                {
                    comment_flag_[i].comment_weight_ += comment_flag_[i].love_weight_;
                }
                else if ((norma_luck_[4] - luck_[4]) > 0)
                {
                    comment_flag_[i].comment_weight_ += norma_luck_[4] - luck_[4];
                }
            }
        }

        //ソート処理(ラムダ式を使うらしい)
        comment_flag_.Sort((a, b) => b.comment_weight_ - a.comment_weight_);


        //五行陰陽関係のコメント探索
        for (int i = 0; i < comment_flag_.Count; ++i)
        {
            if (comment_.Count >= comment_num_elements)
            {
                break;
            }

            if ((comment_flag_[i].advice_type_ != AdviceType.ElementGame)
                && (comment_flag_[i].advice_type_ != AdviceType.Element))
            {
                continue;
            }

            if (comment_flag_[i].comment_weight_ <= 0)
            {
                continue;
            }

            //ところどころコメントがかぶっています．
            switch (comment_flag_[i].comment_identifier_)
            {
                case CommentIdentifier.OverYin:
                    {
                        string comment_buffer = "";
                        bool bad_flag = false;
                        bool foliage_flag = false;
                        bool white_flag = false;
                        for (int j = 0; j < furniture_grid_.Count; ++j)
                        {
                            if ((furniture_grid_[j].material_type().IndexOf(FurnitureGrid.MaterialType.ArtificialFoliage) >= 0)
                                && (furniture_grid_[j].characteristic().IndexOf(FurnitureGrid.Characteristic.Clutter) >= 0))
                            {
                                bad_flag = true;
                                comment_buffer += "人工観葉植物, 乱雑な家具を置かないようにしましょう";
                                break;
                            }
                            else if (furniture_grid_[j].material_type().IndexOf(FurnitureGrid.MaterialType.ArtificialFoliage) >= 0)
                            {
                                bad_flag = true;
                                comment_buffer += "人工観葉植物を置かないようにしましょう";
                                break;
                            }
                            else if ((furniture_grid_[j].characteristic().IndexOf(FurnitureGrid.Characteristic.Clutter) >= 0))
                            {
                                bad_flag = true;
                                comment_buffer += "乱雑な家具を置かないようにしましょう";
                                break;
                            }

                            if (furniture_grid_[j].furniture_type() == FurnitureGrid.FurnitureType.FoliagePlant)
                            {
                                foliage_flag = true;
                            }

                            if (　furniture_grid_[j].color_name().IndexOf(FurnitureGrid.ColorName.White) >= 0 )
                            {
                                white_flag = true;
                            }
                        }

                        if ((bad_flag == false) && (foliage_flag == false))
                        {
                            comment_buffer += "観葉植物で陰気を中和しましょう";
                        }
                        else if ((bad_flag == false) && (white_flag == false))
                        {
                            comment_buffer += "白い家具で陰気を中和しましょう";
                        }
                        else if(bad_flag == false)
                        {
                            comment_buffer += "部屋の陰気を陽気で中和しましょう";

                        }

                        comment_.Add(comment_buffer);
                    }
                    break;
                case CommentIdentifier.OverYang:
                    {
                        string comment_buffer = "";
                        bool foliage_flag = false;
                        bool white_flag = false;
                        for (int j = 0; j < furniture_grid_.Count; ++j)
                        {
                            if (furniture_grid_[j].furniture_type() == FurnitureGrid.FurnitureType.FoliagePlant)
                            {
                                foliage_flag = true;
                            }

                            if (furniture_grid_[j].color_name().IndexOf(FurnitureGrid.ColorName.White) >= 0)
                            {
                                white_flag = true;
                            }
                        }

                        if (foliage_flag == false)
                        {
                            comment_buffer += "観葉植物で陽気を中和しましょう";
                        }
                        else if (white_flag == false)
                        {
                            comment_buffer += "白い家具で陽気を中和しましょう";
                        }
                        else
                        {
                            comment_buffer += "部屋の陽気を陰気で中和しましょう";
                        }
                        comment_.Add(comment_buffer);
                    }
                    break;
                //部屋の方位から受けるパワー関連
                case CommentIdentifier.NorthWeak:
                    comment_.Add("水の気を中心に部屋の気を上げましょう");
                    break;
                case CommentIdentifier.NorthEastWeak:
                    comment_.Add("土の気を中心に部屋の気を上げましょう");
                    break;
                case CommentIdentifier.NorthEastMinus:
                    comment_.Add("陰陽バランスを整えましょう．"); //陰
                    break;
                case CommentIdentifier.EastWeak:
                    comment_.Add("木の気を中心に部屋の気を上げましょう");
                    break;
                case CommentIdentifier.SouthEastWeak:
                    comment_.Add("木の気を中心に部屋の気を上げましょう");
                    break;
                case CommentIdentifier.SouthWeak:
                    comment_.Add("火の気を中心に部屋の気を上げましょう");
                    break;
                case CommentIdentifier.SouthWestWeak:
                    comment_.Add("土の気を中心に部屋の気を上げましょう");
                    break;
                case CommentIdentifier.WestWeak:
                    comment_.Add("金の気を中心に部屋の気を上げましょう");
                    break;
                case CommentIdentifier.NorthWestWeak:
                    comment_.Add("金の気を中心に部屋の気を上げましょう");
                    break;

                //部屋の方位から受けるパワー関連(その方向の気以外)
                case CommentIdentifier.NorthWeakOther:
                    comment_.Add("水の気以外で部屋の気を上げましょう");
                    break;
                case CommentIdentifier.NorthEastWeakOther:
                    comment_.Add("土の気以外で部屋の気を上げましょう");
                    break;
                case CommentIdentifier.EastWeakOther:
                    comment_.Add("木の気以外で部屋の気を上げましょう");
                    break;
                case CommentIdentifier.SouthEastWeakOther:
                    comment_.Add("木の気以外で部屋の気を上げましょう");
                    break;
                case CommentIdentifier.SouthWeakOther:
                    comment_.Add("火の気以外で部屋の気を上げましょう");
                    break;
                case CommentIdentifier.SouthWestWeakOther:
                    comment_.Add("土の気以外で部屋の気を上げましょう");
                    break;
                case CommentIdentifier.WestWeakOther:
                    comment_.Add("金の気以外で部屋の気を上げましょう");
                    break;
                case CommentIdentifier.NorthWestWeakOther:
                    comment_.Add("金の気以外で部屋の気を上げましょう");
                    break;



                //部屋の小方位(部屋の中の方位)から受けるパワー関連
                case CommentIdentifier.SplitNorthWeak:
                    comment_.Add("部屋の北側の水の気を高めましょう");
                    break;
                case CommentIdentifier.SplitNorthEastWeak:
                    comment_.Add("部屋の北東側の土の気を高めましょう");
                    break;
                case CommentIdentifier.SplitNorthEastMinus:
                    comment_.Add("部屋の北東側の陰陽バランスを整えましょう．");
                    break;
                case CommentIdentifier.SplitEastWeak:
                    comment_.Add("部屋の東側の木の気を高めましょう");
                    break;
                case CommentIdentifier.SplitSouthEastWeak:
                    comment_.Add("部屋の南東側の木の気を高めましょう");
                    break;
                case CommentIdentifier.SplitSouthWeak:
                    comment_.Add("部屋の南側の火の気を高めましょう");
                    break;
                case CommentIdentifier.SplitSouthWestWeak:
                    comment_.Add("部屋の南西側の土の気を高めましょう");
                    break;
                case CommentIdentifier.SplitWestWeak:
                    comment_.Add("部屋の西側の金の気を高めましょう");
                    break;
                case CommentIdentifier.SplitNorthWestWeak:
                    comment_.Add("部屋の北西側の金の気を高めましょう");
                    break;


                //ここから部屋の気関係その他
                case CommentIdentifier.NorthWestVain:
                    comment_.Add("金の気が強すぎます"); //傘ね
                    break;



                //気が強すぎる
                case CommentIdentifier.WoodOver:
                    comment_.Add("木の気が強すぎます");
                    break;
                case CommentIdentifier.FireOver:
                    comment_.Add("火の気が強すぎます");
                    break;
                case CommentIdentifier.EarthOver:
                    comment_.Add("土の気が強すぎます");
                    break;
                case CommentIdentifier.MetalOver:
                    comment_.Add("金の気が強すぎます");
                    break;
                case CommentIdentifier.WaterOver:
                    comment_.Add("水の気が強すぎます");
                    break;


                //ここから相生効果に関するコメント
                //北側
                case CommentIdentifier.WoodSoshoNorth:
                    comment_.Add("部屋の北側の木の気が火の気に吸収されすぎています");
                    break;
                case CommentIdentifier.FireSoshoNorth:
                    comment_.Add("部屋の北側の火の気が土の気に吸収されすぎています");
                    break;
                case CommentIdentifier.EarthSoshoNorth:
                    comment_.Add("部屋の北側の土の気が金の気に吸収されすぎています");
                    break;
                case CommentIdentifier.MetalSoshoNorth:
                    comment_.Add("部屋の北側の金の気が水の気に吸収されすぎています");
                    break;
                case CommentIdentifier.WaterSoshoNorth:
                    comment_.Add("部屋の北側の水の気が木の気に吸収されすぎています");
                    break;

                //北東側
                case CommentIdentifier.WoodSoshoNorthEast:
                    comment_.Add("部屋の北東側の木の気が火の気に吸収されすぎています");
                    break;
                case CommentIdentifier.FireSoshoNorthEast:
                    comment_.Add("部屋の北東側の火の気が土の気に吸収されすぎています");
                    break;
                case CommentIdentifier.EarthSoshoNorthEast:
                    comment_.Add("部屋の北東側の土の気が金の気に吸収されすぎています");
                    break;
                case CommentIdentifier.MetalSoshoNorthEast:
                    comment_.Add("部屋の北東側の金の気が水の気に吸収されすぎています");
                    break;
                case CommentIdentifier.WaterSoshoNorthEast:
                    comment_.Add("部屋の北東側の水の気が木の気に吸収されすぎています");
                    break;

                //東側
                case CommentIdentifier.WoodSoshoEast:
                    comment_.Add("部屋の東側の木の気が火の気に吸収されすぎています");
                    break;
                case CommentIdentifier.FireSoshoEast:
                    comment_.Add("部屋の東側の火の気が土の気に吸収されすぎています");
                    break;
                case CommentIdentifier.EarthSoshoEast:
                    comment_.Add("部屋の東側の土の気が金の気に吸収されすぎています");
                    break;
                case CommentIdentifier.MetalSoshoEast:
                    comment_.Add("部屋の東側の金の気が水の気に吸収されすぎています");
                    break;
                case CommentIdentifier.WaterSoshoEast:
                    comment_.Add("部屋の東側の水の気が木の気に吸収されすぎています");
                    break;

                //南東側
                case CommentIdentifier.WoodSoshoSouthEast:
                    comment_.Add("部屋の南東側の木の気が火の気に吸収されすぎています");
                    break;
                case CommentIdentifier.FireSoshoSouthEast:
                    comment_.Add("部屋の南東側の火の気が土の気に吸収されすぎています");
                    break;
                case CommentIdentifier.EarthSoshoSouthEast:
                    comment_.Add("部屋の南東側の土の気が金の気に吸収されすぎています");
                    break;
                case CommentIdentifier.MetalSoshoSouthEast:
                    comment_.Add("部屋の南東側の金の気が水の気に吸収されすぎています");
                    break;
                case CommentIdentifier.WaterSoshoSouthEast:
                    comment_.Add("部屋の南東側の水の気が木の気に吸収されすぎています");
                    break;

                //南側
                case CommentIdentifier.WoodSoshoSouth:
                    comment_.Add("部屋の南側の木の気が火の気に吸収されすぎています");
                    break;
                case CommentIdentifier.FireSoshoSouth:
                    comment_.Add("部屋の南側の火の気が土の気に吸収されすぎています");
                    break;
                case CommentIdentifier.EarthSoshoSouth:
                    comment_.Add("部屋の南側の土の気が金の気に吸収されすぎています");
                    break;
                case CommentIdentifier.MetalSoshoSouth:
                    comment_.Add("部屋の南側の金の気が水の気に吸収されすぎています");
                    break;
                case CommentIdentifier.WaterSoshoSouth:
                    comment_.Add("部屋の南側の水の気が木の気に吸収されすぎています");
                    break;

                //南西側
                case CommentIdentifier.WoodSoshoSouthWest:
                    comment_.Add("部屋の南西側の木の気が火の気に吸収されすぎています");
                    break;
                case CommentIdentifier.FireSoshoSouthWest:
                    comment_.Add("部屋の南西側の火の気が土の気に吸収されすぎています");
                    break;
                case CommentIdentifier.EarthSoshoSouthWest:
                    comment_.Add("部屋の南西側の土の気が金の気に吸収されすぎています");
                    break;
                case CommentIdentifier.MetalSoshoSouthWest:
                    comment_.Add("部屋の南西側の金の気が水の気に吸収されすぎています");
                    break;
                case CommentIdentifier.WaterSoshoSouthWest:
                    comment_.Add("部屋の南西側の水の気が木の気に吸収されすぎています");
                    break;

                //西側
                case CommentIdentifier.WoodSoshoWest:
                    comment_.Add("部屋の西側の木の気が火の気に吸収されすぎています");
                    break;
                case CommentIdentifier.FireSoshoWest:
                    comment_.Add("部屋の西側の火の気が土の気に吸収されすぎています");
                    break;
                case CommentIdentifier.EarthSoshoWest:
                    comment_.Add("部屋の西側の土の気が金の気に吸収されすぎています");
                    break;
                case CommentIdentifier.MetalSoshoWest:
                    comment_.Add("部屋の西側の金の気が水の気に吸収されすぎています");
                    break;
                case CommentIdentifier.WaterSoshoWest:
                    comment_.Add("部屋の西側の水の気が木の気に吸収されすぎています");
                    break;

                //北西側
                case CommentIdentifier.WoodSoshoNorthWest:
                    comment_.Add("部屋の北西側の木の気が火の気に吸収されすぎています");
                    break;
                case CommentIdentifier.FireSoshoNorthWest:
                    comment_.Add("部屋の北西側の火の気が土の気に吸収されすぎています");
                    break;
                case CommentIdentifier.EarthSoshoNorthWest:
                    comment_.Add("部屋の北西側の土の気が金の気に吸収されすぎています");
                    break;
                case CommentIdentifier.MetalSoshoNorthWest:
                    comment_.Add("部屋の北西側の金の気が水の気に吸収されすぎています");
                    break;
                case CommentIdentifier.WaterSoshoNorthWest:
                    comment_.Add("部屋の北西側の水の気が木の気に吸収されすぎています");
                    break;

                //ここから相克効果に関するコメント
                //北側
                case CommentIdentifier.WoodSokokuNorth:
                    comment_.Add("部屋の北側の木の気が金の気，または土の気に相殺されています");
                    break;
                case CommentIdentifier.FireSokokuNorth:
                    comment_.Add("部屋の北側の火の気が水の気，または金の気に相殺されています");
                    break;
                case CommentIdentifier.EarthSokokuNorth:
                    comment_.Add("部屋の北側の土の気が木の気，または水の気に相殺されています");
                    break;
                case CommentIdentifier.MetalSokokuNorth:
                    comment_.Add("部屋の北側の金の気が火の気，または木の気に相殺されています");
                    break;
                case CommentIdentifier.WaterSokokuNorth:
                    comment_.Add("部屋の北側の水の気が土の気，または火の気に相殺されています");
                    break;

                //北東側
                case CommentIdentifier.WoodSokokuNorthEast:
                    comment_.Add("部屋の北東側の木の気が金の気，または土の気に相殺されています");
                    break;
                case CommentIdentifier.FireSokokuNorthEast:
                    comment_.Add("部屋の北東側の火の気が水の気，または金の気に相殺されています");
                    break;
                case CommentIdentifier.EarthSokokuNorthEast:
                    comment_.Add("部屋の北東側の土の気が木の気，または水の気に相殺されています");
                    break;
                case CommentIdentifier.MetalSokokuNorthEast:
                    comment_.Add("部屋の北東側の金の気が火の気，または木の気に相殺されています");
                    break;
                case CommentIdentifier.WaterSokokuNorthEast:
                    comment_.Add("部屋の北東側の水の気が土の気，または火の気に相殺されています");
                    break;

                //東側
                case CommentIdentifier.WoodSokokuEast:
                    comment_.Add("部屋の東側の木の気が金の気，または土の気に相殺されています");
                    break;
                case CommentIdentifier.FireSokokuEast:
                    comment_.Add("部屋の東側の火の気が水の気，または金の気に相殺されています");
                    break;
                case CommentIdentifier.EarthSokokuEast:
                    comment_.Add("部屋の東側の土の気が木の気，または水の気に相殺されています");
                    break;
                case CommentIdentifier.MetalSokokuEast:
                    comment_.Add("部屋の東側の金の気が火の気，または木の気に相殺されています");
                    break;
                case CommentIdentifier.WaterSokokuEast:
                    comment_.Add("部屋の東側の水の気が土の気，または火の気に相殺されています");
                    break;

                //南東側
                case CommentIdentifier.WoodSokokuSouthEast:
                    comment_.Add("部屋の南東側の木の気が金の気，または土の気に相殺されています");
                    break;
                case CommentIdentifier.FireSokokuSouthEast:
                    comment_.Add("部屋の南東側の火の気が水の気，または金の気に相殺されています");
                    break;
                case CommentIdentifier.EarthSokokuSouthEast:
                    comment_.Add("部屋の南東側の土の気が木の気，または水の気に相殺されています");
                    break;
                case CommentIdentifier.MetalSokokuSouthEast:
                    comment_.Add("部屋の南東側の金の気が火の気，または木の気に相殺されています");
                    break;
                case CommentIdentifier.WaterSokokuSouthEast:
                    comment_.Add("部屋の南東側の水の気が土の気，または火の気に相殺されています");
                    break;

                //南側
                case CommentIdentifier.WoodSokokuSouth:
                    comment_.Add("部屋の南側の木の気が金の気，または土の気に相殺されています");
                    break;
                case CommentIdentifier.FireSokokuSouth:
                    comment_.Add("部屋の南側の火の気が水の気，または金の気に相殺されています");
                    break;
                case CommentIdentifier.EarthSokokuSouth:
                    comment_.Add("部屋の南側の土の気が木の気，または水の気に相殺されています");
                    break;
                case CommentIdentifier.MetalSokokuSouth:
                    comment_.Add("部屋の南側の金の気が火の気，または木の気に相殺されています");
                    break;
                case CommentIdentifier.WaterSokokuSouth:
                    comment_.Add("部屋の南側の水の気が土の気，または火の気に相殺されています");
                    break;

                //南西側
                case CommentIdentifier.WoodSokokuSouthWest:
                    comment_.Add("部屋の南西側の木の気が金の気，または土の気に相殺されています");
                    break;
                case CommentIdentifier.FireSokokuSouthWest:
                    comment_.Add("部屋の南西側の火の気が水の気，または金の気に相殺されています");
                    break;
                case CommentIdentifier.EarthSokokuSouthWest:
                    comment_.Add("部屋の南西側の土の気が木の気，または水の気に相殺されています");
                    break;
                case CommentIdentifier.MetalSokokuSouthWest:
                    comment_.Add("部屋の南西側の金の気が火の気，または木の気に相殺されています");
                    break;
                case CommentIdentifier.WaterSokokuSouthWest:
                    comment_.Add("部屋の南西側の水の気が土の気，または火の気に相殺されています");
                    break;

                // 西側
                case CommentIdentifier.WoodSokokuWest:
                    comment_.Add("部屋の西側の木の気が金の気，または土の気に相殺されています");
                    break;
                case CommentIdentifier.FireSokokuWest:
                    comment_.Add("部屋の西側の火の気が水の気，または金の気に相殺されています");
                    break;
                case CommentIdentifier.EarthSokokuWest:
                    comment_.Add("部屋の西側の土の気が木の気，または水の気に相殺されています");
                    break;
                case CommentIdentifier.MetalSokokuWest:
                    comment_.Add("部屋の西側の金の気が火の気，または木の気に相殺されています");
                    break;
                case CommentIdentifier.WaterSokokuWest:
                    comment_.Add("部屋の西側の水の気が土の気，または火の気に相殺されています");
                    break;

                // 北西側
                case CommentIdentifier.WoodSokokuNorthWest:
                    comment_.Add("部屋の北西側の木の気が金の気，または土の気に相殺されています");
                    break;
                case CommentIdentifier.FireSokokuNorthWest:
                    comment_.Add("部屋の北西側の火の気が水の気，または金の気に相殺されています");
                    break;
                case CommentIdentifier.EarthSokokuNorthWest:
                    comment_.Add("部屋の北西側の土の気が木の気，または水の気に相殺されています");
                    break;
                case CommentIdentifier.MetalSokokuNorthWest:
                    comment_.Add("部屋の北西側の金の気が火の気，または木の気に相殺されています");
                    break;
                case CommentIdentifier.WaterSokokuNorthWest:
                    comment_.Add("部屋の北西側の水の気が土の気，または火の気に相殺されています");
                    break;

                //ここから気を上げましょう関連
                //北側
                case CommentIdentifier.WoodWeakNorth:
                    comment_.Add("部屋の北側の木の気を上げましょう");
                    break;
                case CommentIdentifier.FireWeakNorth:
                    comment_.Add("部屋の北側の火の気を上げましょう");
                    break;
                case CommentIdentifier.EarthWeakNorth:
                    comment_.Add("部屋の北側の土の気を上げましょう");
                    break;
                case CommentIdentifier.MetalWeakNorth:
                    comment_.Add("部屋の北側の金の気を上げましょう");
                    break;
                case CommentIdentifier.WaterWeakNorth:
                    comment_.Add("部屋の北側の水の気を上げましょう");
                    break;

                //北東側
                case CommentIdentifier.WoodWeakNorthEast:
                    comment_.Add("部屋の北東側の木の気を上げましょう");
                    break;
                case CommentIdentifier.FireWeakNorthEast:
                    comment_.Add("部屋の北東側の火の気を上げましょう");
                    break;
                case CommentIdentifier.EarthWeakNorthEast:
                    comment_.Add("部屋の北東側の土の気を上げましょう");
                    break;
                case CommentIdentifier.MetalWeakNorthEast:
                    comment_.Add("部屋の北東側の金の気を上げましょう");
                    break;
                case CommentIdentifier.WaterWeakNorthEast:
                    comment_.Add("部屋の北東側の水の気を上げましょう");
                    break;

                //東側
                case CommentIdentifier.WoodWeakEast:
                    comment_.Add("部屋の東側の木の気を上げましょう");
                    break;
                case CommentIdentifier.FireWeakEast:
                    comment_.Add("部屋の東側の火の気を上げましょう");
                    break;
                case CommentIdentifier.EarthWeakEast:
                    comment_.Add("部屋の東側の土の気を上げましょう");
                    break;
                case CommentIdentifier.MetalWeakEast:
                    comment_.Add("部屋の東側の金の気を上げましょう");
                    break;
                case CommentIdentifier.WaterWeakEast:
                    comment_.Add("部屋の東側の水の気を上げましょう");
                    break;

                //南東側
                case CommentIdentifier.WoodWeakSouthEast:
                    comment_.Add("部屋の南東側の木の気を上げましょう");
                    break;
                case CommentIdentifier.FireWeakSouthEast:
                    comment_.Add("部屋の南東側の火の気を上げましょう");
                    break;
                case CommentIdentifier.EarthWeakSouthEast:
                    comment_.Add("部屋の南東側の土の気を上げましょう");
                    break;
                case CommentIdentifier.MetalWeakSouthEast:
                    comment_.Add("部屋の南東側の金の気を上げましょう");
                    break;
                case CommentIdentifier.WaterWeakSouthEast:
                    comment_.Add("部屋の南東側の水の気を上げましょう");
                    break;

                //南側
                case CommentIdentifier.WoodWeakSouth:
                    comment_.Add("部屋の南側の木の気を上げましょう");
                    break;
                case CommentIdentifier.FireWeakSouth:
                    comment_.Add("部屋の南側の火の気を上げましょう");
                    break;
                case CommentIdentifier.EarthWeakSouth:
                    comment_.Add("部屋の南側の土の気を上げましょう");
                    break;
                case CommentIdentifier.MetalWeakSouth:
                    comment_.Add("部屋の南側の金の気を上げましょう");
                    break;
                case CommentIdentifier.WaterWeakSouth:
                    comment_.Add("部屋の南側の水の気を上げましょう");
                    break;

                //南西
                case CommentIdentifier.WoodWeakSouthWest:
                    comment_.Add("部屋の南西側の木の気を上げましょう");
                    break;
                case CommentIdentifier.FireWeakSouthWest:
                    comment_.Add("部屋の南西側の火の気を上げましょう");
                    break;
                case CommentIdentifier.EarthWeakSouthWest:
                    comment_.Add("部屋の南西側の土の気を上げましょう");
                    break;
                case CommentIdentifier.MetalWeakSouthWest:
                    comment_.Add("部屋の南西側の金の気を上げましょう");
                    break;
                case CommentIdentifier.WaterWeakSouthWest:
                    comment_.Add("部屋の南西側の水の気を上げましょう");
                    break;

                //西
                case CommentIdentifier.WoodWeakWest:
                    comment_.Add("部屋の西側の木の気を上げましょう");
                    break;
                case CommentIdentifier.FireWeakWest:
                    comment_.Add("部屋の西側の火の気を上げましょう");
                    break;
                case CommentIdentifier.EarthWeakWest:
                    comment_.Add("部屋の西側の土の気を上げましょう");
                    break;
                case CommentIdentifier.MetalWeakWest:
                    comment_.Add("部屋の西側の金の気を上げましょう");
                    break;
                case CommentIdentifier.WaterWeakWest:
                    comment_.Add("部屋の西側の水の気を上げましょう");
                    break;

                //北西
                case CommentIdentifier.WoodWeakNorthWest:
                    comment_.Add("部屋の北西側の木の気を上げましょう");
                    break;
                case CommentIdentifier.FireWeakNorthWest:
                    comment_.Add("部屋の北西側の火の気を上げましょう");
                    break;
                case CommentIdentifier.EarthWeakNorthWest:
                    comment_.Add("部屋の北西側の土の気を上げましょう");
                    break;
                case CommentIdentifier.MetalWeakNorthWest:
                    comment_.Add("部屋の北西側の金の気を上げましょう");
                    break;
                case CommentIdentifier.WaterWeakNorthWest:
                    comment_.Add("部屋の北西側の水の気を上げましょう");
                    break;


                default:

                    break;

            }

            for (int j = 0; j < comment_.Count - 1; ++j)
            {
                if (comment_[comment_.Count - 1] == comment_[j])
                {
                    comment_.RemoveAt(comment_.Count - 1);
                    break;
                }
            }

        }



        for (int i = 0; i < comment_flag_.Count; ++i)
        {
            if (comment_.Count >= (comment_num_elements + comment_num_bonus))
            {
                break;
            }

            if ((comment_flag_[i].advice_type_ != AdviceType.BonusGame)
                && (comment_flag_[i].advice_type_ != AdviceType.Bonus))
            {
                continue;
            }

            if (comment_flag_[i].comment_weight_ <= 0)
            {
                continue;
            }

            //ところどころコメントがかぶっています．
            switch (comment_flag_[i].comment_identifier_)
            {

                //北関連
                case CommentIdentifier.NorthCold:
                    comment_.Add("あたたかみのある家具を置きましょう");
                    break;

                //ここからベッドの内容
                case CommentIdentifier.BedLiving:
                    comment_.Add("ベッドを置かないようにしましょう");
                    break;
                case CommentIdentifier.BedWorkroom:
                    comment_.Add("ベッドを置かないようにしましょう");
                    break;
                case CommentIdentifier.BedNoBedroom:
                    comment_.Add("最低限ベッドは置きましょう");
                    break;
                case CommentIdentifier.BedNatural:
                    comment_.Add("化学繊維，プラスチックが使われていないベッドを置きましょう");
                    break;
                case CommentIdentifier.BedGapWall:
                    comment_.Add("ベッドと壁の隙間を開けないようにしましょう");
                    break;
                case CommentIdentifier.BedToDoor:
                    comment_.Add("ベッドの枕をドアの正面に向けないようにしましょう");
                    break;
                case CommentIdentifier.BedNearWindow:
                    comment_.Add("ベッドを窓の近くに置かないようにしましょう");
                    break;
                case CommentIdentifier.BedOver:
                    comment_.Add("ベッドが多すぎます");
                    break;
                case CommentIdentifier.BedSouthToNorth:
                    comment_.Add("ベッドの枕の向きを南から北に変えましょう");
                    break;
                case CommentIdentifier.BedSouthToEast:
                    comment_.Add("ベッドの枕の向きを南から東に変えましょう");
                    break;
                case CommentIdentifier.BedWestToNorth:
                    comment_.Add("ベッドの枕の向きを西から北に変えましょう");
                    break;
                case CommentIdentifier.BedWestToEast:
                    comment_.Add("ベッドの枕の向きを西から東に変えましょう");
                    break;
                case CommentIdentifier.BedNorthToEast:
                    comment_.Add("ベッドの枕の向きを北から東に変えましょう");
                    break;
                case CommentIdentifier.BedEastToNorth:
                    comment_.Add("ベッドの枕の向きを東から北に変えましょう");
                    break;


                //ここからタンスの内容
                case CommentIdentifier.CabinetOver:
                    comment_.Add("タンスが多すぎます");
                    break;


                //ここからカーペットの内容
                case CommentIdentifier.CarpetOver:
                    comment_.Add("カーペットが多すぎます");
                    break;


                //ここから机の内容
                case CommentIdentifier.DeskBedroom:
                    comment_.Add("机を置かないようにしましょう");
                    break;
                case CommentIdentifier.DeskNoWorkRoom:
                    comment_.Add("最低限机は置きましょう");
                    break;
                case CommentIdentifier.DeskOver:
                    comment_.Add("机が多すぎます");
                    break;
                case CommentIdentifier.DeskNorthEastToSouth:
                    comment_.Add("机の向きを北，東向きから南向きに変えてみましょう");
                    break;
                case CommentIdentifier.DeskNorthEastToWest:
                    comment_.Add("机の向きを北，東向きから西向きに変えてみましょう");
                    break;
                case CommentIdentifier.DeskSouthToNorthEast:
                    comment_.Add("机の向きを南向きから北, または東向きに変えてみましょう");
                    break;
                case CommentIdentifier.DeskSouthToWest:
                    comment_.Add("机の向きを南向きから西向きに変えてみましょう");
                    break;
                case CommentIdentifier.DeskWestToNorthEast:
                    comment_.Add("机の向きを西向きから北，または東向きに変えてみましょう");
                    break;
                case CommentIdentifier.DeskWestToSouth:
                    comment_.Add("机の向きを西向きから南向きに変えてみましょう");
                    break;

                //ここから観葉植物の内容
                case CommentIdentifier.FoliagePurification:
                    comment_.Add("観葉植物を置きましょう");
                    break;
                case CommentIdentifier.FoliagePlantOver:
                    comment_.Add("観葉植物が多すぎます");
                    break;


                //ここからランプの内容
                case CommentIdentifier.LampOver:
                    comment_.Add("ランプが多すぎます");
                    break;
                case CommentIdentifier.LampNo:
                    comment_.Add("最低限ランプ1個は置きましょう");
                    break;


                //ここからソファーの内容
                case CommentIdentifier.SofaNoLiving:
                    comment_.Add("最低限ソファーは置きましょう");
                    break;
                case CommentIdentifier.SofaSplitWest:
                    comment_.Add("ソファーは西側に置き, 座席を東向きにしましょう");
                    break;
                case CommentIdentifier.SofaToDoor:
                    comment_.Add("悪い運気を下げるか，ソファーの座席をドアに向けないようにしましょう");
                    break;
                case CommentIdentifier.SofaOver:
                    comment_.Add("ソファーが多すぎます");
                    break;


                //ここからテーブルの内容
                case CommentIdentifier.TableNoLiving:
                    comment_.Add("最低限テーブルは置きましょう");
                    break;
                case CommentIdentifier.TableOver:
                    comment_.Add("テーブルが多すぎます");
                    break;

                //ここから家電の内容
                case CommentIdentifier.ElectronicsSouth:
                    comment_.Add("部屋の南側に家電を置かないようにしましょう");
                    break;
                case CommentIdentifier.ElectronicsNoEast:
                    comment_.Add("部屋の東側に家電を置きましょう");
                    break;
                case CommentIdentifier.TVNoToWest:
                    comment_.Add("部屋の東側のテレビは画面を西向きにしましょう");
                    break;
                case CommentIdentifier.ElectronicsOver:
                    comment_.Add("家電が多すぎます");
                    break;


                //ここから家具の数量の内容
                case CommentIdentifier.FurnitureFew:
                    comment_.Add("家具が少なすぎます");
                    break;
                case CommentIdentifier.FurnitureOver:
                    comment_.Add("家具が多すぎます");
                    break;



                //ここから色特性
                //白関連
                case CommentIdentifier.WhiteColorPurification:
                    comment_.Add("白い家具を最低限1個置きましょう");
                    break;
                case CommentIdentifier.WhiteColorResetYinYang:
                    comment_.Add("白い家具で部屋の陰陽を中和しましょう");
                    break;

                //黒関連
                case CommentIdentifier.BlackStrengthening:
                    comment_.Add("家具の色を見直すか，黒い家具を消しましょう");
                    break;
                case CommentIdentifier.BlackNoStrengthening:
                    comment_.Add("黒い家具を置きましょう");
                    break;
                case CommentIdentifier.BlackNoGreemWarm:
                    comment_.Add("黒，青の家具を置くときは温かみのある家具や緑の家具と合わせましょう");
                    break;
                case CommentIdentifier.BlackInteger:
                    comment_.Add("黒い家具を増やしましょう");
                    break;


                //灰色関連
                case CommentIdentifier.GrayNorthWest:
                    comment_.Add("灰色，濃い灰色，銀色の家具を増やしましょう");
                    break;
                case CommentIdentifier.GraySplitNorthWest:
                    comment_.Add("部屋の北西側に灰色，濃い灰色，銀色の家具を増やしましょう");
                    break;

                //赤関連
                case CommentIdentifier.RedOne:
                    comment_.Add("赤い家具を最低限1個置きましょう");
                    break;
                case CommentIdentifier.RedOver:
                    comment_.Add("赤い家具が多すぎます");
                    break;

                //ピンク関連
                case CommentIdentifier.PinkColorOne:
                    comment_.Add("ピンクの家具を最低限1個置きましょう");
                    break;
                case CommentIdentifier.PinkColorNoOrange:
                    comment_.Add("オレンジの家具を最低限1個置きましょう");
                    break;
                case CommentIdentifier.PinkBed:
                    comment_.Add("ピンクの家具を増やしましょう");
                    break;
                case CommentIdentifier.PinkNorth:
                    comment_.Add("ピンクの家具を増やしましょう");
                    break;
                case CommentIdentifier.PinkSplitNorth:
                    comment_.Add("部屋の北側にピンクの家具を増やしましょう");
                    break;


                //青関連
                case CommentIdentifier.BlueColorOne:
                    comment_.Add("青い家具を最低限1個置きましょう");
                    break;
                case CommentIdentifier.BlueInteger:
                    comment_.Add("青, 水色の家具を増やしましょう");
                    break;
             

                //水色関連
                case CommentIdentifier.LightBlueColorNoOrange:
                    comment_.Add("オレンジの家具を最低限1個置きましょう");
                    break;


                //オレンジ関連
                case CommentIdentifier.OrangeColorNoPink:
                    comment_.Add("ピンクの家具を最低限1個置きましょう");
                    break;
                case CommentIdentifier.OrangeColorNoLightBlue:
                    comment_.Add("水色の家具を最低限1個置きましょう");
                    break;
                case CommentIdentifier.OrangeSouthEast:
                    comment_.Add("オレンジの家具を増やしましょう");
                    break;
                case CommentIdentifier.OrangeSplitSouthEast:
                    comment_.Add("部屋の南東側にオレンジの家具を増やしましょう");
                    break;


                //黄色関連
                case CommentIdentifier.YellowBrownOcherOne:
                    comment_.Add("黄色か茶色か黄土色の家具を最低限1個置きましょう");
                    break;

                //緑

                //黄緑

                //ベージュ関連
                case CommentIdentifier.BeigeCreamNorthWest:
                    comment_.Add("ベージュ, クリーム色の家具を増やしましょう");
                    break;
                case CommentIdentifier.BeigeCreamSplitNorthWest:
                    comment_.Add("部屋の北西側にベージュ，クリーム色の家具を置きましょう");
                    break;

                //クリーム色

                //茶
                //黄土色
                //金
                case CommentIdentifier.GoldOne:
                    comment_.Add("金色の家具を最低限1個置きましょう");
                    break;
                case CommentIdentifier.GoldBad:
                    comment_.Add("金色の家具が，悪い運気を増幅させています．");
                    break;

                //銀
                case CommentIdentifier.SilverInteger:
                    comment_.Add("銀色の家具を増やしましょう");
                    break;

                //紫

                //材質関連
                //人工観葉植物

                //木製
                case CommentIdentifier.WoodNaturalCottonBedroom:
                    comment_.Add("木製，天然繊維，綿製の家具を増やしましょう");
                    break;
                //紙
                //革
                //天然繊維
                //化学繊維
                case CommentIdentifier.CemicalPlusticOne:
                    comment_.Add("化学繊維，プラスチック製の家具を置かないようにしましょう");
                    break;
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
                //背が高い
                case CommentIdentifier.HighFormNorthEast:
                    comment_.Add("背が高い家具を増やしましょう");
                    break;
                case CommentIdentifier.HighFormSplitNorthEast:
                    comment_.Add("部屋の北東側に背の高い家具を増やしましょう");
                    break;
                case CommentIdentifier.HighOver:
                    comment_.Add("背が高い家具が多すぎます");
                    break;

                //背が低い
                case CommentIdentifier.LowFormSouthWest:
                    comment_.Add("背の低い家具を増やしましょう");
                    break;
                case CommentIdentifier.LowFormSplitSouthWest:
                    comment_.Add("部屋の南西側に背の低い家具を増やしましょう");
                    break;
                //縦長
                //横長

                //正方形
                case CommentIdentifier.SquareOne:
                    comment_.Add("正方形，または長方形の家具を最低限1個置きましょう．");
                    break;
                case CommentIdentifier.SquareBad:
                    comment_.Add("悪い運気を消すか，正方形，長方形の家具をなくしましょう．");
                    break;

                //長方形
                case CommentIdentifier.RectangleMulti:
                    comment_.Add("");
                    break;

                //円形
                case CommentIdentifier.RoundBad:
                    comment_.Add("丸，または楕円形の家具を最低限1個置いて，悪い運気を浄化しましょう．");
                    break;
                case CommentIdentifier.RoundOne:
                    comment_.Add("丸，楕円形の家具をなくしましょう．");
                    break;
                //楕円形
                case CommentIdentifier.EllipseMulti:
                    comment_.Add("");
                    break;

                //三角形

                //尖っている
                case CommentIdentifier.SharpBad:
                    comment_.Add("悪い運気を消すか，尖った家具をなくしましょう．");
                    break;



                //特性関連

                //高級そう
                case CommentIdentifier.LuxuryNorthWest:
                    comment_.Add("高級そうな家具を増やしましょう");
                    break;
                case CommentIdentifier.LuxurySplitNorthWest:
                    comment_.Add("部屋の北西側に高級そうな家具を増やしましょう");
                    break;
                case CommentIdentifier.LuxuryZeroNorthWest:
                    comment_.Add("高級そうなの家具を最低限1個置きましょう");
                    break;

                //音が出る
                case CommentIdentifier.SoundEast:
                    comment_.Add("音が出る家具や，いい匂いのする家具，風に関連する家具を増やしましょう");
                    break;
                case CommentIdentifier.SoundSouthEast:
                    comment_.Add("音が出る家具や，いい匂いのする家具，風に関連する家具を増やしましょう");
                    break;
                case CommentIdentifier.SoundSplitEastSouthEast:
                    comment_.Add("部屋の東側や南東側に音が出る家具や，いい匂いのする家具，風に関連する家具を増やしましょう");
                    break;

                //(いい)におい
               

                //発光
                //硬い
                //やわらかい
                //温かみ
                case CommentIdentifier.WarmInteger:
                    comment_.Add("温かみのある家具を増やしましょう");
                    break;

                //冷たさ
                case CommentIdentifier.ColdNorth:
                    comment_.Add("冷たい家具を置かないようにしましょう");
                    break;

                case CommentIdentifier.ColdSplitNorth:
                    comment_.Add("部屋の北側に冷たい家具を置かないようにしましょう");
                    break;

                //花関連
                //風関連

                //西洋風
                case CommentIdentifier.WesternWest:
                    comment_.Add("西洋風な家具を増やしましょう");
                    break;
                case CommentIdentifier.WesternSplitWest:
                    comment_.Add("部屋の西側に西洋風な家具を増やしましょう");
                    break;

                //乱雑

                default:

                    break;

            }

            for (int j = 0; j < comment_.Count - 1; ++j)
            {
                if (comment_[comment_.Count - 1] == comment_[j])
                {
                    comment_.RemoveAt(comment_.Count - 1);
                    break;
                }
            }

        }
        comment_flag_.Clear();

    }
}
