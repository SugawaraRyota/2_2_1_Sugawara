//このファイルはEvaluationの分割ファイルであり評価の際のコメントを呼び出す関数である
//
//
// comment_flag_とflag_weight_を参照にして
// comment_とcomment_weight_を設定する
// comment_weight_がより高い5つの文が表示される. 
//
// 基本的には風水コメントその2の条件をもとにコメントを出力する


using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//ゲーム終了時のワンポイントアドバイス
public partial class Evaluation : MonoBehaviour
{
    partial void Comment()
    {
        int comment_num_elements = 2; //五行陰陽関係コメント数
        int comment_num_bonus = 2; //ボーナス点関係コメント数

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
                        string comment_buffer = "部屋の陰気がつよく，仕事運，人気運，健康運，恋愛運が下がっています．";
                        bool bad_flag = false;
                        bool foliage_flag = false;
                        bool white_flag = false;
                        for (int j = 0; j < furniture_grid_.Count; ++j)
                        {

                            if ((furniture_grid_[j].material_type().IndexOf(FurnitureGrid.MaterialType.ArtificialFoliage) >= 0)
                                && (furniture_grid_[j].characteristic().IndexOf(FurnitureGrid.Characteristic.Clutter) >= 0))
                            {
                                bad_flag = true;
                                comment_buffer += "人工観葉植物, 乱雑な家具は部屋に大きな陰気をもたらします．";
                                break;
                            }
                            else if (furniture_grid_[j].material_type().IndexOf(FurnitureGrid.MaterialType.ArtificialFoliage) >= 0)
                            {
                                bad_flag = true;
                                comment_buffer += "人工観葉植物は部屋に大きな陰気をもたらします．";
                                break;
                            }
                            else if ((furniture_grid_[j].characteristic().IndexOf(FurnitureGrid.Characteristic.Clutter) >= 0))
                            {
                                bad_flag = true;
                                comment_buffer += "乱雑な家具は部屋に大きな陰気をもたらします．";
                                break;
                            }

                            if (furniture_grid_[j].furniture_type() == FurnitureGrid.FurnitureType.FoliagePlant)
                            {
                                foliage_flag = true;
                            }

                            if(furniture_grid_[j].color_name().IndexOf(FurnitureGrid.ColorName.White) >= 0)
                            {
                                white_flag = true;
                            }
                        }

                        if ((bad_flag == false) && (foliage_flag == false))
                        {
                            comment_buffer += "観葉植物は周囲の陰陽のバランスを整えますので是非置きましょう. ";
                        }
                        else if((bad_flag == false) && (white_flag == false))
                        {
                            comment_buffer += "白い家具は部屋全体の陰陽のバランスを整えますので是非置きましょう. ";
                        }

                        comment_.Add(comment_buffer);
                    }
                    break;
                case CommentIdentifier.OverYang:
                    {
                        string comment_buffer = "部屋の陽気がつよく，仕事運，健康運，恋愛運が下がっています．";
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
                            comment_buffer += "観葉植物は周囲の陰陽のバランスを整えますので是非置きましょう. ";
                        }
                        else if ((white_flag == false))
                        {
                            comment_buffer += "白い家具は部屋全体の陰陽のバランスを整えますので是非置きましょう. ";
                        }
                        comment_.Add(comment_buffer);
                    }
                    break;
                //部屋の方位から受けるパワー関連
                case CommentIdentifier.NorthWeak:
                    comment_.Add("北の部屋では水の気を中心に部屋の気を上げることで北方位が司る金運，恋愛運を上げることができます．");
                    break;
                case CommentIdentifier.NorthEastWeak:
                    comment_.Add("北東の部屋では土の気を中心に部屋の気を上げることで北東方位が司る全体運を上げることができます．");
                    break;
                case CommentIdentifier.NorthEastMinus:
                    comment_.Add("北東の部屋では陰陽のバランスが取れていないと, 方位が司る運気のパワーが逆効果になってしまいます．"); //陰
                    break;
                case CommentIdentifier.EastWeak:
                    comment_.Add("東の部屋では木の気を中心に部屋の気を上げることで東方位が司る仕事運を上げることができます．");
                    break;
                case CommentIdentifier.SouthEastWeak:
                    comment_.Add("南東の部屋では木の気を中心に部屋の気を上げることで南東方位が司る人気運，恋愛運を上げることができます．");
                    break;
                case CommentIdentifier.SouthWeak:
                    comment_.Add("南の部屋では火の気を中心に部屋の気を上げることで南方位が司る人気運，健康運, 恋愛運を上げることができます．");
                    break;
                case CommentIdentifier.SouthWestWeak:
                    comment_.Add("南西の部屋では土の気を中心に部屋の気を上げることで南西方位が司る仕事運，人気運，健康運を上げることができます．");
                    break;
                case CommentIdentifier.WestWeak:
                    comment_.Add("西の部屋では金の気を中心に部屋の気を上げることで西方位が司る金運，恋愛運を上げることができます．");
                    break;
                case CommentIdentifier.NorthWestWeak:
                    comment_.Add("北西の部屋では金の気を中心に部屋の気を上げることで北西方位が司る仕事運，金運を上げることができます．");
                    break;


                //部屋の方位から受けるパワー関連(その方向の気以外)
                case CommentIdentifier.NorthWeakOther:
                    comment_.Add("北方位のパワーを享受する水の気は十分にありますので他の属性の気で部屋の気を上げて，北方位が司る金運，恋愛運をさらに受け取りましょう．");
                    break;
                case CommentIdentifier.NorthEastWeakOther:
                    comment_.Add("北東方位のパワーを享受する土の気は十分にありますので他の属性の気で部屋の気を上げて，北東方位が司る全体運をさらに受け取りましょう．");
                    break;
                case CommentIdentifier.EastWeakOther:
                    comment_.Add("東方位のパワーを享受する木の気は十分にありますので他の属性の気で部屋の気を上げて，東方位が司る仕事運をさらに受け取りましょう．");
                    break;
                case CommentIdentifier.SouthEastWeakOther:
                    comment_.Add("南東方位のパワーを享受する木の気は十分にありますので他の属性の気で部屋の気を上げて，南東方位が司る人気運，恋愛運をさらに受け取りましょう．");
                    break;
                case CommentIdentifier.SouthWeakOther:
                    comment_.Add("南方位のパワーを享受する火の気は十分にありますので他の属性の気で部屋の気を上げて，南方位が司る人気運，健康運, 恋愛運をさらに受け取りましょう．");
                    break;
                case CommentIdentifier.SouthWestWeakOther:
                    comment_.Add("南西方位のパワーを享受する土の気は十分にありますので他の属性の気で部屋の気を上げて，南西方位が司る仕事運，人気運，健康運をさらに受け取りましょう. ");
                    break;
                case CommentIdentifier.WestWeakOther:
                    comment_.Add("西方位のパワーを享受する金の気は十分にありますので他の属性の気で部屋の気を上げて，西方位が司る金運, 恋愛運をさらに受け取りましょう. ");
                    break;
                case CommentIdentifier.NorthWestWeakOther:
                    comment_.Add("北西方位のパワーを享受する金の気は十分にありますので他の属性の気で部屋の気を上げて，西方位が司る金運, 仕事運をさらに受け取りましょう. ");
                    break;



                //部屋の小方位(部屋の中の方位)から受けるパワー関連
                case CommentIdentifier.SplitNorthWeak:
                    comment_.Add("部屋の北側の水の気を高めて，北方位が司る金運，恋愛運を受け取りましょう．");
                    break;
                case CommentIdentifier.SplitNorthEastWeak:
                    comment_.Add("部屋の北東側の土の気を高めて，北東方位が司る全体運を受け取りましょう．");
                    break;
                case CommentIdentifier.SplitNorthEastMinus:
                    comment_.Add("部屋の北東側の陰陽バランスが悪く，北東方位が司る全体運が悪影響を受けてます．");
                    break;
                case CommentIdentifier.SplitEastWeak:
                    comment_.Add("部屋の東側の木の気を高めて，東方位が司る仕事運を受け取りましょう．");
                    break;
                case CommentIdentifier.SplitSouthEastWeak:
                    comment_.Add("部屋の南東側の木の気を高めて，南東方位が司る人気運，恋愛運を受け取りましょう．");
                    break;
                case CommentIdentifier.SplitSouthWeak:
                    comment_.Add("部屋の南側の火の気を高めて，南方位が司る人気運，健康運，恋愛運を受け取りましょう．");
                    break;
                case CommentIdentifier.SplitSouthWestWeak:
                    comment_.Add("部屋の南西側の土の気を高めて，南西方位が司る仕事運，人気運，健康運を受け取りましょう．");
                    break;
                case CommentIdentifier.SplitWestWeak:
                    comment_.Add("部屋の西側の金の気を高めて，西方位が司る金運，恋愛運を受け取りましょう．");
                    break;
                case CommentIdentifier.SplitNorthWestWeak:
                    comment_.Add("部屋の北西側の金の気を高めて，北西方位が司る仕事運，金運を受け取りましょう．");
                    break;


                //ここから部屋の気関係その他
                case CommentIdentifier.NorthWestVain:
                    comment_.Add("北西の部屋では金の気があまりにも強すぎると仕事運，人気運が下がりますので上げすぎに注意しましょう. "); //傘ね
                    break;
                case CommentIdentifier.SouthPurification:
                    comment_.Add("南の部屋では，部屋の気が高いと悪い運気を浄化してくれます．今，悪い運気が高まっている部屋レイアウトなので，気を高めて悪い気を浄化してもらいましょう．");
                    break;


                //気が強すぎる
                case CommentIdentifier.WoodOver:
                    comment_.Add("部屋の木の気が強すぎて，仕事運に悪影響を及ぼしています．相生，相克効果など利用して木の気を抑えましょう．");
                    break;
                case CommentIdentifier.FireOver:
                    comment_.Add("部屋の火の気が強すぎて，仕事運，健康運，恋愛運に悪影響を及ぼしています．相生，相克効果などを利用して火の気を抑えましょう．");
                    break;
                case CommentIdentifier.EarthOver:
                    comment_.Add("部屋の土の気が強すぎて，健康運に悪影響を及ぼしています．相生，相克効果などを利用して土の気を抑えましょう．");
                    break;
                case CommentIdentifier.MetalOver:
                    comment_.Add("部屋の金の気が強すぎて，金運に悪影響を及ぼしています．相生，相克効果などを利用して金の気を抑えましょう．");
                    break;
                case CommentIdentifier.WaterOver:
                    comment_.Add("部屋の水の気が強すぎて，健康運，金運，恋愛運に悪影響を及ぼしています．相生，相克効果などを利用して水の気を抑えましょう．");
                    break;


                //ここから相生効果に関するコメント
                case CommentIdentifier.WoodSosho:
                    comment_.Add("木の気は火の気に対し相生関係にあり，相性は良いですが，木の気が必要以上に火の気に吸収されています．木の気と火の気を合わせるのもほどほどにしましょう．");
                    break;
                case CommentIdentifier.FireSosho:
                    comment_.Add("火の気は土の気に対し相生関係にあり，相性は良いですが，火の気が必要以上に土の気に吸収されています．火の気と土の気を合わせるのもほどほどにしましょう．");
                    break;
                case CommentIdentifier.EarthSosho:
                    comment_.Add("土の気は金の気に対し相生関係にあり，相性は良いですが，土の気が必要以上に金の気に吸収されています．土の気と金の気を合わせるのもほどほどにしましょう．");
                    break;
                case CommentIdentifier.MetalSosho:
                    comment_.Add("金の気は水の気に対し相生関係にあり，相性は良いですが，金の気が必要以上に水の気に吸収されています．金の気と水の気を合わせるのもほどほどにしましょう．");
                    break;
                case CommentIdentifier.WaterSosho:
                    comment_.Add("水の気は木の気に対し相生関係にあり，相性は良いですが，水の気が必要以上に木の気に吸収されています．水の気と木の気を合わせるのもほどほどにしましょう．");
                    break;

                //ここから相克効果に関するコメント
                case CommentIdentifier.WoodSokoku:
                    comment_.Add("木の気は金の気，土の気と相性が悪いので，それらの気が強い場所や，家具の近くに木の気が強い家具を置くのはなるべく控えましょう．");
                    break;
                case CommentIdentifier.FireSokoku:
                    comment_.Add("火の気は水の気，金の気と相性が悪いので，それらの気が強い場所や，家具の近くに木の気が強い家具を置くのはなるべく控えましょう．");
                    break;
                case CommentIdentifier.EarthSokoku:
                    comment_.Add("土の気は木の気，水の気と相性が悪いので，それらの気が強い場所や, 家具の近くに木の気が強い家具を置くのはなるべく控えましょう．");
                    break;
                case CommentIdentifier.MetalSokoku:
                    comment_.Add("金の気は火の気，木の気と相性が悪いので，それらの気が強い場所や，家具の近くに木の気が強い家具を置くのはなるべく控えましょう．");
                    break;
                case CommentIdentifier.WaterSokoku:
                    comment_.Add("水の気は土の気，火の気と相性が悪いので，それらの気が強い場所や，家具の近くに木の気が強い家具を置くのはなるべく控えましょう．");
                    break;

                //ここから五行通常
                case CommentIdentifier.WoodWeakNorth:
                    comment_.Add("最後の部屋の気の状態に，さらに木の気を多く注ぎ込めば，運気アップを見込めます．");
                    break;
                case CommentIdentifier.FireWeakNorth:
                    comment_.Add("最後の部屋の気の状態に，さらに火の気を多く注ぎ込めば，運気アップを見込めます．");
                    break;
                case CommentIdentifier.EarthWeakNorth:
                    comment_.Add("最後の部屋の気の状態に，さらに土の気を多く注ぎ込めば，運気アップを見込めます．");
                    break;
                case CommentIdentifier.MetalWeakNorth:
                    comment_.Add("最後の部屋の気の状態に，さらに金の気を多く注ぎ込めば，運気アップを見込めます．");
                    break;
                case CommentIdentifier.WaterWeakNorth:
                    comment_.Add("最後の部屋の気の状態に，さらに水の気を多く注ぎ込めば，運気アップを見込めます．");
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
                //部屋の種類関連
                case CommentIdentifier.BedroomMulti:
                    comment_.Add("寝室は，運気の影響を特に受けやすい部屋なので，悪い運気を生み出さないように，細心の注意を払いましょう．");
                    break;
                case CommentIdentifier.WorkroomMulti:
                    comment_.Add("仕事部屋は，仕事運の影響を受けやすくなっていますので，仕事運の悪い運気を生み出さないように，細心の注意を払いましょう．");
                    break;
                case CommentIdentifier.LivingMulti:
                    comment_.Add("リビングは，仕事運，健康運，人気運の影響を受けやすくなっていますので，それらの悪い運気を生み出さないように，細心の注意を払いましょう．");
                    break;

                //北関連
                case CommentIdentifier.NorthCold:
                    comment_.Add("北の部屋は元々，健康運，人気運を下げてしまいますが，温かみのある家具を置くことで北方位の悪い効果を打ち消すことができます．");
                    break;

                //ここからベッドの内容
                case CommentIdentifier.BedLiving:
                    comment_.Add("リビングにベッドがあると仕事運，人気運，健康運が大きく下がりますので置かないようにしましょう．");
                    break;
                case CommentIdentifier.BedWorkroom:
                    comment_.Add("仕事部屋にベッドがあると仕事運，健康運が大きく下がりますので置かないようにしましょう．");
                    break;
                case CommentIdentifier.BedNoBedroom:
                    comment_.Add("寝室にベッドがないと健康運を中心に運気が大きく下がりますので最低1個は置くようにしましょう．");
                    break;
                case CommentIdentifier.BedNatural:
                    comment_.Add("化学繊維，プラスチックが使われているベッドでは，ベッドが持っている健康運上昇のパワーを受けることができません．");
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
                    comment_.Add("ベッドが多すぎて運気が全体的に大きく下がっています．ベッドの数は" + limit_bed_.ToString() + "個までにしましょう. ");
                    break;
                case CommentIdentifier.BedSouthDirection:
                    comment_.Add("南枕のベッドは仕事運，健康運を下げますので北枕か東枕にしましょう．");
                    break;
                case CommentIdentifier.BedWestDirection:
                    comment_.Add("西枕のベッドは仕事運，健康運を下げますので北枕か東枕にしましょう．");
                    break;

                //ここからタンスの内容
                case CommentIdentifier.CabinetOver:
                    comment_.Add("タンスが多すぎて運気が全体的に大きく下がっています．タンスの数は" + limit_cabinet_.ToString() + "個までにしましょう．");
                    break;


                //ここからカーペットの内容
                case CommentIdentifier.CarpetOver:
                    comment_.Add("カーペットが多すぎて運気が全体的に大きく下がっています．カーペットの数は" + limit_carpet_.ToString() + "個までにしましょう．");
                    break;


                //ここから机の内容
                case CommentIdentifier.DeskBedroom:
                    comment_.Add("寝室に机があると仕事運，健康運が大きくに下がりますので置かないようにしましょう．");
                    break;
                case CommentIdentifier.DeskNoWorkRoom:
                    comment_.Add("仕事部屋に机がないと仕事運が極めて大幅に下がりますので最低1個は置くようにしましょう．");
                    break;
                case CommentIdentifier.DeskOver:
                    comment_.Add("机が多すぎて運気が全体的に大きく下がっています．机の数は" + limit_desk_.ToString() + "個までにしましょう．");
                    break;
                case CommentIdentifier.DeskNoNorthEast:
                    comment_.Add("机の向きを北向き，または東向きにすると仕事運を上げることができます．");
                    break;
                case CommentIdentifier.DeskNoSouth:
                    comment_.Add("机の向きを南向きにすると人気運を上げることができます．");
                    break;
                case CommentIdentifier.DeskNoWest:
                    comment_.Add("机の向きを西向きにすると金運を上げることができます．");
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
                    comment_.Add("部屋に悪い運気が流れ込んできているので，観葉植物で悪い運気を浄化しましょう．");
                    break;
                case CommentIdentifier.FoliagePlantOver:
                    comment_.Add("観葉植物が多すぎて運気が全体的に大きく下がっています．観葉植物の数は" + limit_foliage_.ToString() + "個までにしましょう．");
                    break;


                //ここからランプの内容
                case CommentIdentifier.LampOver:
                    comment_.Add("照明(ランプ)が多すぎて運気が全体的に大きく下がっています．ランプの数は" + limit_lamp_.ToString() + "個までにしましょう．");
                    break;
                case CommentIdentifier.LampNo:
                    comment_.Add("部屋に照明(ランプ)がないのは論外です．金運以外の運気が大きく下がってしまいます．");
                    break;


                //ここからソファーの内容
                case CommentIdentifier.SofaNoLiving:
                    comment_.Add("リビングにソファーがないと仕事運, 人気運，特に健康運が大きく下がりますので最低1個は置くようにしましょう．");
                    break;
                case CommentIdentifier.SofaSplitWest:
                    comment_.Add("ソファーは西側に，座席を東向きにして置くと，運気を全体的に上げることができます．");
                    break;
                case CommentIdentifier.SofaToDoor:
                    comment_.Add("悪い運気を下げるか，ソファーの座席をドアに向けないようにしましょう");
                    break;
                case CommentIdentifier.SofaOver:
                    comment_.Add("ソファーが多すぎて運気が全体的に大きく下がっています．ソファーの数は" + limit_sofa_.ToString() + "個までにしましょう．");
                    break;


                //ここからテーブルの内容
                case CommentIdentifier.TableNoLiving:
                    comment_.Add("リビングにテーブルがないと仕事運, 人気運，特に健康運が大きく下がりますので最低1個は置くようにしましょう．");
                    break;
                case CommentIdentifier.TableOver:
                    comment_.Add("テーブルが多すぎて運気が全体的に大きく下がっています．テーブルの数は" + limit_table_.ToString() + "個までにしましょう．");
                    break;

                //ここから家電の内容
                case CommentIdentifier.ElectronicsSouth:
                    comment_.Add("家電類が部屋の南側に置いてあると仕事運，健康運，恋愛運が下がってしまいます．");
                    break;
                case CommentIdentifier.ElectronicsNoEast:
                    comment_.Add("家電類は部屋の東側に置くと仕事運，人気運，健康運を上げることができます．");
                    break;
                case CommentIdentifier.TVNoToWest:
                    comment_.Add("部屋の東側のテレビは画面を西向きにすると，仕事運，人気運，健康運がさらに上がります．");
                    break;
                case CommentIdentifier.ElectronicsOver:
                    comment_.Add("家電類が多すぎて運気が全体的に大きく下がっています．家電類の数は" + limit_electronics_.ToString() + "個までにしましょう．");
                    break;


                //ここから家具の数量の内容
                case CommentIdentifier.FurnitureFew:
                    comment_.Add("いくら何でも家具が少なすぎます．最低でも" + limit_furniture_few_.ToString() + "個は置かないと運気が大きく下がってしまいます．");
                    break;
                case CommentIdentifier.FurnitureOver:
                    comment_.Add("いくら何でも家具が多すぎます．家具の数は" + limit_furniture_.ToString() + "個までにしないと運気が大きく下がってしまいます．");
                    break;



                //ここから色特性
                //白関連
                case CommentIdentifier.WhiteColorPurification:
                    comment_.Add("部屋に悪い運気が流れ込んできているので，白い家具を最低限1個置いて，悪い運気を浄化しましょう．");
                    break;
                case CommentIdentifier.WhiteColorResetYinYang:
                    comment_.Add("白い家具で部屋の陰陽を中和しましょう");
                    break;

                //黒関連
                case CommentIdentifier.BlackStrengthening:
                    comment_.Add("家具の色によって悪い運気が生み出され，それが黒い家具によって増幅されています．色の取り扱いに注意しましょう．");
                    break;
                case CommentIdentifier.BlackNoStrengthening:
                    comment_.Add("家具の色によって生み出された良い運気は，黒い家具を置くことで増幅させることができます．");
                    break;
                case CommentIdentifier.BlackNoGreemWarm:
                    comment_.Add("黒や青の家具を1つでも置くと人気運，健康運，恋愛運を下げてしまいますが緑，または温かみのある家具を置くことでその悪い効果を打ち消すことができます．");
                    break;
                case CommentIdentifier.BlackInteger:
                    comment_.Add("黒い家具は置けば置くほど金運を少しずつ上げることができます．");
                    break;


                //灰色関連
                case CommentIdentifier.GrayNorthWest:
                    comment_.Add("北西の部屋では，灰色，濃い灰色，銀色の家具を置けば置くほど仕事運，金運を少しずつ上げることができます．");
                    break;
                case CommentIdentifier.GraySplitNorthWest:
                    comment_.Add("灰色や濃い灰色，銀色の家具は部屋の北西側に置くと仕事運，金運が上がります．");
                    break;

                //赤関連
                case CommentIdentifier.RedOne:
                    comment_.Add("赤い家具を1つでも置けば，仕事運，健康運を上げることができます．");
                    break;
                case CommentIdentifier.RedOver:
                    comment_.Add("赤い家具が多すぎて仕事運，健康運に悪影響がでています．赤い家具は" + limit_red_color_.ToString() + "個までにしましょう．");
                    break;

                //ピンク関連
                case CommentIdentifier.PinkColorOne:
                    comment_.Add("ピンクの家具を1つでも置けば，恋愛運を大きく上げることができます．");
                    break;
                case CommentIdentifier.PinkColorNoOrange:
                    comment_.Add("ピンクとオレンジを合わせることで恋愛運が上がります．せっかくピンクの家具がありますのでオレンジの家具を置きましょう．");
                    break;
                case CommentIdentifier.PinkBed:
                    comment_.Add("寝室では，ピンクの家具を置けば置くほど恋愛運を少しずつ上げることができます．");
                    break;
                case CommentIdentifier.PinkNorth:
                    comment_.Add("北の部屋では，ピンクの家具を置けば置くほど恋愛運を少しずつ上げることができます．");
                    break;
                case CommentIdentifier.PinkSplitNorth:
                    comment_.Add("ピンクの家具は部屋の北側に置くと恋愛運が上がります．");
                    break;

                //青関連
                case CommentIdentifier.BlueColorOne:
                    comment_.Add("青い家具を1つでも置けば，仕事運を上げることができます．");
                    break;
                case CommentIdentifier.BlueInteger:
                    comment_.Add("青や水色の家具は置けば置くほど健康運を少しずつ上げることができます．");
                    break;
            

                //水色関連
                case CommentIdentifier.LightBlueColorNoOrange:
                    comment_.Add("水色とオレンジを合わせることで健康運が上がります．せっかく水色の家具がありますのでオレンジの家具を置きましょう．");
                    break;


                //オレンジ関連
                case CommentIdentifier.OrangeColorNoPink:
                    comment_.Add("ピンクとオレンジを合わせることで恋愛運が上がります．せっかくオレンジの家具がありますのでピンクの家具を置きましょう．");
                    break;
                case CommentIdentifier.OrangeColorNoLightBlue:
                    comment_.Add("水色とオレンジを合わせることで健康運が上がります．せっかくオレンジの家具がありますので水色の家具を置きましょう．");
                    break;
                case CommentIdentifier.OrangeSouthEast:
                    comment_.Add("南東の部屋では，オレンジの家具を置けば置くほど人気運，恋愛運を少しずつ上げることができます．");
                    break;
                case CommentIdentifier.OrangeSplitSouthEast:
                    comment_.Add("オレンジの家具は部屋の南東側に置くと人気運，恋愛運が上がります．");
                    break;

                //黄色関連
                case CommentIdentifier.YellowBrownOcherOne:
                    comment_.Add("黄色か茶色か黄土色の家具を最低限1個置くことで，部屋の良い運気をさらに増幅できます．");
                    break;

                //緑

                //黄緑

                //ベージュ関連
                case CommentIdentifier.BeigeCreamNorthWest:
                    comment_.Add("北西の部屋では，ベージュ，クリーム色の家具を置けば置くほど仕事運，金運を少しずつ上げることができます．");
                    break;
                case CommentIdentifier.BeigeCreamSplitNorthWest:
                    comment_.Add("ベージュ，クリーム色の家具は部屋の北西側に置くと仕事運，金運が上がります．");
                    break;
       //クリーム
                //茶
                //黄土色
                //金
                case CommentIdentifier.GoldOne:
                    comment_.Add("金色の家具を最低限1個おくことで，金運を中心に，部屋の良い運気をさらに増幅できます．");
                    break;
                case CommentIdentifier.GoldBad:
                    comment_.Add("金運を中心とした部屋の悪い気が，金色の家具によって増幅されてますので，悪い運気を消すか，金色の家具をなくしましょう．");
                    break;


                //銀
                case CommentIdentifier.SilverInteger:
                    comment_.Add("銀色の家具は置けば置くほど金運を少しずつ上げることができます．");
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
                    comment_.Add("北東の部屋では，背の高い家具を置けば置くほど運気を全体的に少しずつ上げることができます．");
                    break;
                case CommentIdentifier.HighFormSplitNorthEast:
                    comment_.Add("背の高い家具は部屋の北東側に置くと運気が全体的に少し上がります．");
                    break;
                case CommentIdentifier.HighOver:
                    comment_.Add("背が高い家具が多すぎます");
                    break;

                //背が低い
                case CommentIdentifier.LowFormSouthWest:
                    comment_.Add("南西の部屋では，背の低い家具を置けば置くほど仕事運，人気運，健康運を少しずつ上げることができます．");
                    break;
                case CommentIdentifier.LowFormSplitSouthWest:
                    comment_.Add("背の低い家具は部屋の南西側に置くと仕事運，人気運，健康運が上がります．");
                    break;
                //縦長
                //横長

                //正方形
                case CommentIdentifier.SquareOne:
                    comment_.Add("正方形，または長方形の家具を最低限1個置くことで，部屋の良い運気をさらに浄化できます．");
                    break;
                case CommentIdentifier.SquareBad:
                    comment_.Add("部屋の悪い気が，正方形，長方形の家具によって増幅されてますので，悪い運気を消すか，正方形，長方形の家具をなくしましょう．");
                    break;

                //長方形
                case CommentIdentifier.RectangleMulti:
                    comment_.Add("");
                    break;

                //円形
                case CommentIdentifier.RoundBad:
                    comment_.Add("丸，または楕円形の家具を最低限1個置くことで，部屋の悪い運気をさらに浄化できます．");
                    break;
                case CommentIdentifier.RoundOne:
                    comment_.Add("部屋の良い気が，丸，楕円形の家具によって縮小されてますので，丸，楕円形の家具をなくしましょう．");
                    break;
                //楕円形
                case CommentIdentifier.EllipseMulti:
                    comment_.Add("");
                    break;

                //三角形

                //尖っている
                case CommentIdentifier.SharpBad:
                    comment_.Add("部屋の悪い気が，尖った家具によって増幅されてますので，悪い運気を消すか，尖った家具をなくしましょう．");
                    break;

                //奇抜な形状



                //特性関連

                //高級そう
                case CommentIdentifier.LuxuryNorthWest:
                    comment_.Add("北西の部屋では，高級そうな家具を置けば置くほど仕事運，金運を少しずつ上げることができます．");
                    break;
                case CommentIdentifier.LuxurySplitNorthWest:
                    comment_.Add("高級そうな家具は部屋の北西側に置くと仕事運，金運が上がります．");
                    break;
                case CommentIdentifier.LuxuryZeroNorthWest:
                    comment_.Add("北西の部屋に高級そうな家具が1つもないと，仕事運，人気運が下がってしまいます．");
                    break;

                //音が出る
                case CommentIdentifier.SoundEast:
                    comment_.Add("東の部屋では，音が出る，いい匂いがする，風に関連する家具置けば置くほど人気運，恋愛運を少しずつ上げることができます．");
                    break;

                case CommentIdentifier.SoundSouthEast:
                    comment_.Add("南東の部屋では，音が出る，いい匂いがする，風に関連する家具置けば置くほど人気運，恋愛運を少しずつ上げることができます．");
                    break;
                case CommentIdentifier.SoundSplitEastSouthEast:
                    comment_.Add("音が出る，いい匂いがする，風に関連する家具を部屋の東側や南東側に置くと人気運，恋愛運が上がります．");
                        break;

                //(いい)におい
            

                //発光
                //硬い
                //やわらかい
                //温かみ
                case CommentIdentifier.WarmInteger:
                    comment_.Add("温かみのある家具は置けば置くほど健康運を少しずつ上げることができます．");
                    break;

                //冷たさ
                case CommentIdentifier.ColdNorth:
                    comment_.Add("北の部屋では，冷たい家具を置くと健康運がどんどん下がってしまいます．");
                    break;

                case CommentIdentifier.ColdSplitNorth:
                    comment_.Add("冷たい家具は，部屋の北側に置いてしまうと健康運を下げてしまいます．");
                    break;

                //花関連
                //風関連
              

                //西洋風
                case CommentIdentifier.WesternWest:
                    comment_.Add("西の部屋では，西洋風の家具を置けば置くほど金運を少しずつ上げることができます．");
                    break;
                case CommentIdentifier.WesternSplitWest:
                    comment_.Add("西洋風の家具は部屋の西側に置くと金運が上がります．");
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
    }


}
