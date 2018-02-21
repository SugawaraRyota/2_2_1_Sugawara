//このクラスはFurnitureGridクラスの分割部分であり，テーブルのグリッドデータを生成するGetGridDataTableメソッドが実装されている
//
//テーブルのFurnitureTypeはTable
//
//方位の指定は特になし
//
//parametaの長さは1
//parameta_[0] = ダミー
//
//(ここからは自分の勝手な判断)
//
//table_5_gridは除外

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; //UnityEventを使用するため
using UnityEngine.EventSystems; //

public partial class FurnitureGrid : MonoBehaviour
{
    partial void GetGridDataTable(int furniture_ID)
    {
        switch (furniture_ID)
        {
            case 1:
            default:
                {
                    //テーブルタイプ1(木製)

                    object_type_ = ObjectType.Normal;
                    children_number_ = 3;
                    center_point_ = new int[2] { 13, 13 }; //中心のグリッド座標
                    put_point_ = new int[2] { 13, 13 }; //上に乗る家具の中心が合わせる座標
                                                        //使用する頂点グリッド
                    vertices_number_ = 12;
                    grid_point_ = new int[vertices_number_][];
                    grid_point_[0] = new int[2] { 0, 8 }; //0
                    grid_point_[1] = new int[2] { 0, 18 }; //1
                    grid_point_[2] = new int[2] { 8, 8 }; //2
                    grid_point_[3] = new int[2] { 8, 18 }; //3

                    grid_point_[4] = new int[2] { 8, 0 }; //4
                    grid_point_[5] = new int[2] { 8, 26 }; //5
                    grid_point_[6] = new int[2] { 18, 0 }; //6
                    grid_point_[7] = new int[2] { 18, 26 }; //7

                    grid_point_[8] = new int[2] { 18, 8 }; //8
                    grid_point_[9] = new int[2] { 18, 18 }; //9
                    grid_point_[10] = new int[2] { 26, 8 }; //10
                    grid_point_[11] = new int[2] { 26, 18 }; //11

                    texture_ = Resources.Load<Texture2D>("table/table_1_grid"); //テクスチャはそのうち変える
                    Debug.Log(texture_);
                    uv_coordinate_ = new Vector2[vertices_number_];
                    uv_coordinate_[0] = new Vector2(0.0F, 0.308F); //0
                    uv_coordinate_[1] = new Vector2(0.0F, 0.692F); //1
                    uv_coordinate_[2] = new Vector2(0.308F, 0.308F); //2
                    uv_coordinate_[3] = new Vector2(0.308F, 0.692F); //3

                    uv_coordinate_[4] = new Vector2(0.308F, 0.0F); //4
                    uv_coordinate_[5] = new Vector2(0.308F, 1.0F); //5
                    uv_coordinate_[6] = new Vector2(0.692F, 0.0F); //6
                    uv_coordinate_[7] = new Vector2(0.692F, 1.0F); //7

                    uv_coordinate_[8] = new Vector2(0.692F, 0.308F); //8
                    uv_coordinate_[9] = new Vector2(0.692F, 0.692F); //9
                    uv_coordinate_[10] = new Vector2(1.0F, 0.308F); //10
                    uv_coordinate_[11] = new Vector2(1.0F, 0.692F); //11

                    children_grid_ = new GameObject[children_number_];
                    //頂点インデックス生成
                    triangles_ = new int[children_number_][];
                    triangles_[0] = new int[4] { 0, 1, 2, 3 };
                    triangles_[1] = new int[4] { 4, 5, 6, 7 };
                    triangles_[2] = new int[4] { 8, 9, 10, 11 };

                    //枠線
                    outline_index_ = new int[24] { 0, 2, 2, 4, 4, 6, 6, 8, 8, 10, 10, 11, 11, 9, 9, 7, 7, 5, 5, 3, 3, 1, 1, 0 };
                    blueflag_index_ = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

                    //パラメータの設定
                    parameta_[0] = 0; //ダミー

                    //-------------------------------------------------

                    color_name_.Add(ColorName.Red);
                    color_name_weight_.Add(15);

                    //-------------------------------------------------

                    material_type_.Add(MaterialType.Plastic);
                    material_type_weight_.Add(15);

                    //-------------------------------------------------

                    pattern_type_.Add(PatternType.Dot);
                    pattern_type_weight_.Add(6);

                    //-------------------------------------------------

                    form_type_.Add(FormType.Round);
                    form_type_weight_.Add(15);

                    //--------------------------------------------------

                    characteristic_.Add(Characteristic.Hard);
                    characteristic_weight_.Add(4);

                    //--------------------------------------------------

                    break;
                }

            case 2:
                {
                    //テーブルタイプ2(木製)

                    object_type_ = ObjectType.Normal;
                    children_number_ = 1;
                    center_point_ = new int[2] { 10, 12 }; //中心のグリッド座標
                    put_point_ = new int[2] { 10, 12 }; //上に乗る家具の中心が合わせる座標
                                                        //使用する頂点グリッド
                    vertices_number_ = 4;
                    grid_point_ = new int[vertices_number_][];
                    grid_point_[0] = new int[2] { 0, 0 }; //0
                    grid_point_[1] = new int[2] { 0, 24 }; //1
                    grid_point_[2] = new int[2] { 20, 0 }; //2
                    grid_point_[3] = new int[2] { 20, 24 }; //3

                    texture_ = Resources.Load<Texture2D>("table/table_2_grid"); //テクスチャはそのうち変える
                    Debug.Log(texture_);
                    uv_coordinate_ = new Vector2[vertices_number_];
                    uv_coordinate_[0] = new Vector2(0.0F, 0.0F); //0
                    uv_coordinate_[1] = new Vector2(0.0F, 1.0F); //1
                    uv_coordinate_[2] = new Vector2(1.0F, 0.0F); //2
                    uv_coordinate_[3] = new Vector2(1.0F, 1.0F); //3

                    children_grid_ = new GameObject[children_number_];
                    //頂点インデックス生成
                    triangles_ = new int[children_number_][];
                    triangles_[0] = new int[4] { 0, 1, 2, 3 };

                    //枠線
                    outline_index_ = new int[8] { 0, 2, 2, 3, 3, 1, 1, 0 };
                    blueflag_index_ = new bool[4] { false, false, false, false };

                    //パラメータの設定
                    parameta_[0] = 0; //ダミー

                    //-------------------------------------------------

                    color_name_.Add(ColorName.Brown);
                    color_name_weight_.Add(11);

                    color_name_.Add(ColorName.Red);
                    color_name_weight_.Add(4);

                    color_name_.Add(ColorName.White);
                    color_name_weight_.Add(2);

                    color_name_.Add(ColorName.LightGreen);
                    color_name_weight_.Add(1);

                    color_name_.Add(ColorName.Orange);
                    color_name_weight_.Add(1);

                    color_name_.Add(ColorName.Silver);
                    color_name_weight_.Add(1);

                    //-------------------------------------------------

                    material_type_.Add(MaterialType.Wooden);
                    material_type_weight_.Add(11);

                    material_type_.Add(MaterialType.Leather);
                    material_type_weight_.Add(4);

                    material_type_.Add(MaterialType.Ceramic);
                    material_type_weight_.Add(2);

                    material_type_.Add(MaterialType.Metal);
                    material_type_weight_.Add(1);

                    //-------------------------------------------------

                    //-------------------------------------------------

                    form_type_.Add(FormType.Rectangle);
                    form_type_weight_.Add(14);

                    //--------------------------------------------------

                    characteristic_.Add(Characteristic.Hard);
                    characteristic_weight_.Add(4);

                    characteristic_.Add(Characteristic.Smell);
                    characteristic_weight_.Add(3);

                    //--------------------------------------------------

                    break;
                }

            case 3:
                {
                    //テーブルタイプ2(木製)

                    object_type_ = ObjectType.Normal;
                    children_number_ = 3;
                    center_point_ = new int[2] { 12, 12 }; //中心のグリッド座標
                    put_point_ = new int[2] { 12, 12 }; //上に乗る家具の中心が合わせる座標
                                                        //使用する頂点グリッド

                    vertices_number_ = 12;
                    grid_point_ = new int[vertices_number_][];
                    grid_point_[0] = new int[2] { 0, 7 }; //0
                    grid_point_[1] = new int[2] { 0, 17 }; //1
                    grid_point_[2] = new int[2] { 7, 7 }; //2
                    grid_point_[3] = new int[2] { 7, 17 }; //3

                    grid_point_[4] = new int[2] { 7, 0 }; //4
                    grid_point_[5] = new int[2] { 7, 24 }; //5
                    grid_point_[6] = new int[2] { 17, 0 }; //6
                    grid_point_[7] = new int[2] { 17, 24 }; //7

                    grid_point_[8] = new int[2] { 17, 7 }; //8
                    grid_point_[9] = new int[2] { 17, 17 }; //9
                    grid_point_[10] = new int[2] { 24, 7 }; //10
                    grid_point_[11] = new int[2] { 24, 17 }; //11

                    texture_ = Resources.Load<Texture2D>("table/table_3_grid"); //テクスチャはそのうち変える
                    Debug.Log(texture_);
                    uv_coordinate_ = new Vector2[vertices_number_];
                    uv_coordinate_[0] = new Vector2(0.0F, 0.292F); //0
                    uv_coordinate_[1] = new Vector2(0.0F, 0.708F); //1
                    uv_coordinate_[2] = new Vector2(0.292F, 0.292F); //2
                    uv_coordinate_[3] = new Vector2(0.292F, 0.708F); //3

                    uv_coordinate_[4] = new Vector2(0.292F, 0.0F); //4
                    uv_coordinate_[5] = new Vector2(0.292F, 1.0F); //5
                    uv_coordinate_[6] = new Vector2(0.708F, 0.0F); //6
                    uv_coordinate_[7] = new Vector2(0.708F, 1.0F); //7

                    uv_coordinate_[8] = new Vector2(0.708F, 0.292F); //8
                    uv_coordinate_[9] = new Vector2(0.708F, 0.708F); //9
                    uv_coordinate_[10] = new Vector2(1.0F, 0.292F); //10
                    uv_coordinate_[11] = new Vector2(1.0F, 0.708F); //11

                    children_grid_ = new GameObject[children_number_];
                    //頂点インデックス生成
                    triangles_ = new int[children_number_][];
                    triangles_[0] = new int[4] { 0, 1, 2, 3 };
                    triangles_[1] = new int[4] { 4, 5, 6, 7 };
                    triangles_[2] = new int[4] { 8, 9, 10, 11 };

                    //枠線
                    outline_index_ = new int[24] { 0, 2, 2, 4, 4, 6, 6, 8, 8, 10, 10, 11, 11, 9, 9, 7, 7, 5, 5, 3, 3, 1, 1, 0 };
                    blueflag_index_ = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

                    //パラメータの設定
                    parameta_[0] = 0; //ダミー

                    //-------------------------------------------------

                    color_name_.Add(ColorName.DarkGray);
                    color_name_weight_.Add(15);

                    //-------------------------------------------------

                    material_type_.Add(MaterialType.Plastic);
                    material_type_weight_.Add(15);

                    //-------------------------------------------------

                    pattern_type_.Add(PatternType.Stripe);
                    pattern_type_weight_.Add(6);

                    //-------------------------------------------------

                    form_type_.Add(FormType.Rectangle);
                    form_type_weight_.Add(15);

                    //--------------------------------------------------

                    characteristic_.Add(Characteristic.Hard);
                    characteristic_weight_.Add(4);

                    //--------------------------------------------------

                    break;
                }

            case 4:
                {
                    //テーブルタイプ4

                    object_type_ = ObjectType.Normal;
                    children_number_ = 1;
                    center_point_ = new int[2] { 8, 6 }; //中心のグリッド座標
                    put_point_ = new int[2] { 8, 6 }; //上に乗る家具の中心が合わせる座標
                                                      //使用する頂点グリッド
                    vertices_number_ = 4;
                    grid_point_ = new int[vertices_number_][];
                    grid_point_[0] = new int[2] { 0, 0 }; //0
                    grid_point_[1] = new int[2] { 0, 13 }; //1
                    grid_point_[2] = new int[2] { 16, 0 }; //2
                    grid_point_[3] = new int[2] { 16, 13 }; //3

                    texture_ = Resources.Load<Texture2D>("table/table_4_grid"); //テクスチャはそのうち変える
                    Debug.Log(texture_);
                    uv_coordinate_ = new Vector2[vertices_number_];
                    uv_coordinate_[0] = new Vector2(0.0F, 0.0F); //0
                    uv_coordinate_[1] = new Vector2(0.0F, 1.0F); //1
                    uv_coordinate_[2] = new Vector2(1.0F, 0.0F); //2
                    uv_coordinate_[3] = new Vector2(1.0F, 1.0F); //3

                    children_grid_ = new GameObject[children_number_];
                    //頂点インデックス生成
                    triangles_ = new int[children_number_][];
                    triangles_[0] = new int[4] { 0, 1, 2, 3 };

                    //枠線
                    outline_index_ = new int[8] { 0, 2, 2, 3, 3, 1, 1, 0 };
                    blueflag_index_ = new bool[4] { false, false, false, false };

                    //パラメータの設定
                    parameta_[0] = 0; //ダミー

                    //-------------------------------------------------

                    color_name_.Add(ColorName.Brown);
                    color_name_weight_.Add(15);

                    //-------------------------------------------------

                    material_type_.Add(MaterialType.Wooden);
                    material_type_weight_.Add(11);

                    material_type_.Add(MaterialType.NaturalFibre);
                    material_type_weight_.Add(4);

                    //-------------------------------------------------

                    pattern_type_.Add(PatternType.Irregularity);
                    pattern_type_weight_.Add(3);

                    pattern_type_.Add(PatternType.Leaf);
                    pattern_type_weight_.Add(3);

                    pattern_type_.Add(PatternType.Wave);
                    pattern_type_weight_.Add(2);

                    //-------------------------------------------------

                    form_type_.Add(FormType.Rectangle);
                    form_type_weight_.Add(14);

                    //--------------------------------------------------

                    characteristic_.Add(Characteristic.Hard);
                    characteristic_weight_.Add(4);

                    //--------------------------------------------------

                    break;
                }

            case 5:
                {
                    //テーブルタイプ5
                    object_type_ = ObjectType.Normal;
                    children_number_ = 3;
                    center_point_ = new int[2] { 12, 12 }; //中心のグリッド座標
                    put_point_ = new int[2] { 12, 12 }; //上に乗る家具の中心が合わせる座標
                                                        //使用する頂点グリッド
                    vertices_number_ = 12;
                    grid_point_ = new int[vertices_number_][];
                    grid_point_[0] = new int[2] { 0, 5 }; //0
                    grid_point_[1] = new int[2] { 0, 18 }; //1
                    grid_point_[2] = new int[2] { 5, 5 }; //2
                    grid_point_[3] = new int[2] { 5, 18 }; //3

                    grid_point_[4] = new int[2] { 5, 0 }; //4
                    grid_point_[5] = new int[2] { 5, 23 }; //5
                    grid_point_[6] = new int[2] { 19, 0 }; //6
                    grid_point_[7] = new int[2] { 19, 23 }; //7

                    grid_point_[8] = new int[2] { 19, 5 }; //8
                    grid_point_[9] = new int[2] { 19, 18 }; //9
                    grid_point_[10] = new int[2] { 24, 5 }; //10
                    grid_point_[11] = new int[2] { 24, 18 }; //11

                    texture_ = Resources.Load<Texture2D>("table/table_5_grid"); //テクスチャはそのうち変える
                    Debug.Log(texture_);
                    uv_coordinate_ = new Vector2[vertices_number_];
                    uv_coordinate_[0] = new Vector2(0.0F, 0.217F); //0
                    uv_coordinate_[1] = new Vector2(0.0F, 0.783F); //1
                    uv_coordinate_[2] = new Vector2(0.208F, 0.217F); //2
                    uv_coordinate_[3] = new Vector2(0.208F, 0.783F); //3

                    uv_coordinate_[4] = new Vector2(0.208F, 0.0F); //4
                    uv_coordinate_[5] = new Vector2(0.208F, 1.0F); //5
                    uv_coordinate_[6] = new Vector2(0.792F, 0.0F); //6
                    uv_coordinate_[7] = new Vector2(0.792F, 1.0F); //7

                    uv_coordinate_[8] = new Vector2(0.792F, 0.217F); //8
                    uv_coordinate_[9] = new Vector2(0.792F, 0.783F); //9
                    uv_coordinate_[10] = new Vector2(1.0F, 0.217F); //10
                    uv_coordinate_[11] = new Vector2(1.0F, 0.783F); //11

                    children_grid_ = new GameObject[children_number_];
                    //頂点インデックス生成
                    triangles_ = new int[children_number_][];
                    triangles_[0] = new int[4] { 0, 1, 2, 3 };
                    triangles_[1] = new int[4] { 4, 5, 6, 7 };
                    triangles_[2] = new int[4] { 8, 9, 10, 11 };

                    //枠線
                    outline_index_ = new int[24] { 0, 2, 2, 4, 4, 6, 6, 8, 8, 10, 10, 11, 11, 9, 9, 7, 7, 5, 5, 3, 3, 1, 1, 0 };
                    blueflag_index_ = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

                    //パラメータの設定
                    parameta_[0] = 0; //ダミー

                    //-------------------------------------------------

                    color_name_.Add(ColorName.Brown);
                    color_name_weight_.Add(10);

                    color_name_.Add(ColorName.Ocher);
                    color_name_weight_.Add(8);

                    color_name_.Add(ColorName.Green);
                    color_name_weight_.Add(2);

                    color_name_.Add(ColorName.Black);
                    color_name_weight_.Add(1);

                    color_name_.Add(ColorName.Purple);
                    color_name_weight_.Add(1);

                    color_name_.Add(ColorName.White);
                    color_name_weight_.Add(1);

                    //-------------------------------------------------

                    material_type_.Add(MaterialType.Wooden);
                    material_type_weight_.Add(11);

                    material_type_.Add(MaterialType.NaturalFibre);
                    material_type_weight_.Add(2);

                    material_type_.Add(MaterialType.Ceramic);
                    material_type_weight_.Add(2);

                    material_type_.Add(MaterialType.Glass);
                    material_type_weight_.Add(2);

                    material_type_.Add(MaterialType.Water);
                    material_type_weight_.Add(1);

                    //-------------------------------------------------

                    pattern_type_.Add(PatternType.Dot);
                    pattern_type_weight_.Add(6);

                    pattern_type_.Add(PatternType.Wave);
                    pattern_type_weight_.Add(2);

                    //-------------------------------------------------

                    form_type_.Add(FormType.Square);
                    form_type_weight_.Add(15);

                    //--------------------------------------------------

                    characteristic_.Add(Characteristic.Hard);
                    characteristic_weight_.Add(4);


                    characteristic_.Add(Characteristic.Flower);
                    characteristic_weight_.Add(3);


                    characteristic_.Add(Characteristic.Western);
                    characteristic_weight_.Add(3);


                    break;
                }

            case 6:
                {
                    //テーブルタイプ6

                    object_type_ = ObjectType.Normal;
                    children_number_ = 3;
                    center_point_ = new int[2] { 15, 11 }; //中心のグリッド座標
                    put_point_ = new int[2] { 15, 11 }; //上に乗る家具の中心が合わせる座標
                                                        //使用する頂点グリッド
                    vertices_number_ = 12;
                    grid_point_ = new int[vertices_number_][];
                    grid_point_[0] = new int[2] { 0, 5 }; //0
                    grid_point_[1] = new int[2] { 0, 18 }; //1
                    grid_point_[2] = new int[2] { 3, 5 }; //2
                    grid_point_[3] = new int[2] { 3, 18 }; //3

                    grid_point_[4] = new int[2] { 3, 0 }; //4
                    grid_point_[5] = new int[2] { 3, 22 }; //5
                    grid_point_[6] = new int[2] { 27, 0 }; //6
                    grid_point_[7] = new int[2] { 27, 22 }; //7

                    grid_point_[8] = new int[2] { 27, 5 }; //8
                    grid_point_[9] = new int[2] { 27, 18 }; //9
                    grid_point_[10] = new int[2] { 30, 5 }; //10
                    grid_point_[11] = new int[2] { 30, 18 }; //11

                    texture_ = Resources.Load<Texture2D>("table/table_6_grid"); //テクスチャはそのうち変える
                    Debug.Log(texture_);
                    uv_coordinate_ = new Vector2[vertices_number_];
                    uv_coordinate_[0] = new Vector2(0.0F, 0.227F); //0
                    uv_coordinate_[1] = new Vector2(0.0F, 0.818F); //1
                    uv_coordinate_[2] = new Vector2(0.1F, 0.227F); //2
                    uv_coordinate_[3] = new Vector2(0.1F, 0.818F); //3

                    uv_coordinate_[4] = new Vector2(0.1F, 0.0F); //4
                    uv_coordinate_[5] = new Vector2(0.1F, 1.0F); //5
                    uv_coordinate_[6] = new Vector2(0.9F, 0.0F); //6
                    uv_coordinate_[7] = new Vector2(0.9F, 1.0F); //7

                    uv_coordinate_[8] = new Vector2(0.9F, 0.227F); //8
                    uv_coordinate_[9] = new Vector2(0.9F, 0.818F); //9
                    uv_coordinate_[10] = new Vector2(1.0F, 0.227F); //10
                    uv_coordinate_[11] = new Vector2(1.0F, 0.818F); //11

                    children_grid_ = new GameObject[children_number_];
                    //頂点インデックス生成
                    triangles_ = new int[children_number_][];
                    triangles_[0] = new int[4] { 0, 1, 2, 3 };
                    triangles_[1] = new int[4] { 4, 5, 6, 7 };
                    triangles_[2] = new int[4] { 8, 9, 10, 11 };

                    //枠線
                    outline_index_ = new int[24] { 0, 2, 2, 4, 4, 6, 6, 8, 8, 10, 10, 11, 11, 9, 9, 7, 7, 5, 5, 3, 3, 1, 1, 0 };
                    blueflag_index_ = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

                    //パラメータの設定
                    parameta_[0] = 0; //ダミー

                    //-------------------------------------------------

                    color_name_.Add(ColorName.Brown);
                    color_name_weight_.Add(8);

                    color_name_.Add(ColorName.Cream);
                    color_name_weight_.Add(8);

                    color_name_.Add(ColorName.Gold);
                    color_name_weight_.Add(2);

                    color_name_.Add(ColorName.LightGreen);
                    color_name_weight_.Add(1);

                    //-------------------------------------------------

                    material_type_.Add(MaterialType.Wooden);
                    material_type_weight_.Add(11);

                    material_type_.Add(MaterialType.Ceramic);
                    material_type_weight_.Add(4);

                    material_type_.Add(MaterialType.Metal);
                    material_type_weight_.Add(1);

                    //-------------------------------------------------

                    pattern_type_.Add(PatternType.Zigzag);
                    pattern_type_weight_.Add(6);

                    pattern_type_.Add(PatternType.Wave);
                    pattern_type_weight_.Add(2);

                    //-------------------------------------------------

                    form_type_.Add(FormType.Rectangle);
                    form_type_weight_.Add(15);

                    //--------------------------------------------------

                    characteristic_.Add(Characteristic.Hard);
                    characteristic_weight_.Add(4);

                    characteristic_.Add(Characteristic.Luxury);
                    characteristic_weight_.Add(3);

                    characteristic_.Add(Characteristic.Western);
                    characteristic_weight_.Add(3);

                    characteristic_.Add(Characteristic.Light);
                    characteristic_weight_.Add(3);

                    //--------------------------------------------------

                    break;
                }

            case 7:
                {
                    //テーブルタイプ7

                    object_type_ = ObjectType.Normal;
                    children_number_ = 3;
                    center_point_ = new int[2] { 16, 9 }; //中心のグリッド座標
                    put_point_ = new int[2] { 16, 9 }; //上に乗る家具の中心が合わせる座標
                                                       //使用する頂点グリッド
                    vertices_number_ = 12;
                    grid_point_ = new int[vertices_number_][];
                    grid_point_[0] = new int[2] { 0, 0 }; //0
                    grid_point_[1] = new int[2] { 0, 14 }; //1
                    grid_point_[2] = new int[2] { 5, 0 }; //2
                    grid_point_[3] = new int[2] { 5, 14 }; //3

                    grid_point_[4] = new int[2] { 5, 0 }; //4
                    grid_point_[5] = new int[2] { 5, 20 }; //5
                    grid_point_[6] = new int[2] { 26, 0 }; //6
                    grid_point_[7] = new int[2] { 26, 20 }; //7

                    grid_point_[8] = new int[2] { 26, 0 }; //8
                    grid_point_[9] = new int[2] { 26, 12 }; //9
                    grid_point_[10] = new int[2] { 34, 0 }; //10
                    grid_point_[11] = new int[2] { 34, 12 }; //11

                    texture_ = Resources.Load<Texture2D>("table/table_7_grid"); //テクスチャはそのうち変える
                    Debug.Log(texture_);
                    uv_coordinate_ = new Vector2[vertices_number_];
                    uv_coordinate_[0] = new Vector2(0.0F, 0.0F); //0
                    uv_coordinate_[1] = new Vector2(0.0F, 0.7F); //1
                    uv_coordinate_[2] = new Vector2(0.147F, 0.0F); //2
                    uv_coordinate_[3] = new Vector2(0.147F, 0.7F); //3

                    uv_coordinate_[4] = new Vector2(0.147F, 0.0F); //4
                    uv_coordinate_[5] = new Vector2(0.147F, 1.0F); //5
                    uv_coordinate_[6] = new Vector2(0.765F, 0.0F); //6
                    uv_coordinate_[7] = new Vector2(0.765F, 1.0F); //7

                    uv_coordinate_[8] = new Vector2(0.765F, 0.0F); //8
                    uv_coordinate_[9] = new Vector2(0.765F, 0.6F); //9
                    uv_coordinate_[10] = new Vector2(1.0F, 0.0F); //10
                    uv_coordinate_[11] = new Vector2(1.0F, 0.6F); //11

                    children_grid_ = new GameObject[children_number_];
                    //頂点インデックス生成
                    triangles_ = new int[children_number_][];
                    triangles_[0] = new int[4] { 0, 1, 2, 3 };
                    triangles_[1] = new int[4] { 4, 5, 6, 7 };
                    triangles_[2] = new int[4] { 8, 9, 10, 11 };

                    //枠線
                    outline_index_ = new int[24] { 0, 2, 2, 4, 4, 6, 6, 8, 8, 10, 10, 11, 11, 9, 9, 7, 7, 5, 5, 3, 3, 1, 1, 0 };
                    blueflag_index_ = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

                    //パラメータの設定
                    parameta_[0] = 0; //ダミー

                    //-------------------------------------------------

                    color_name_.Add(ColorName.Brown);
                    color_name_weight_.Add(5);

                    color_name_.Add(ColorName.Silver);
                    color_name_weight_.Add(5);

                    color_name_.Add(ColorName.White);
                    color_name_weight_.Add(5);



                    //-------------------------------------------------
                    material_type_.Add(MaterialType.ChemicalFibre);
                    material_type_weight_.Add(5);

                    material_type_.Add(MaterialType.Metal);
                    material_type_weight_.Add(5);

                    material_type_.Add(MaterialType.Wooden);
                    material_type_weight_.Add(5);


                    //-------------------------------------------------

                    pattern_type_.Add(PatternType.Wave);
                    pattern_type_weight_.Add(2);

                    //-------------------------------------------------

                    form_type_.Add(FormType.Square);
                    form_type_weight_.Add(15);

                    //--------------------------------------------------

                    characteristic_.Add(Characteristic.Hard);
                    characteristic_weight_.Add(5);

                    //--------------------------------------------------

                    break;
                }

            case 8:
                {
                    //テーブルタイプ8

                    object_type_ = ObjectType.Normal;
                    children_number_ = 3;
                    center_point_ = new int[2] { 12, 12 }; //中心のグリッド座標
                    put_point_ = new int[2] { 12, 12 }; //上に乗る家具の中心が合わせる座標
                                                        //使用する頂点グリッド
                    vertices_number_ = 12;
                    grid_point_ = new int[vertices_number_][];
                    grid_point_[0] = new int[2] { 0, 7 }; //0
                    grid_point_[1] = new int[2] { 0, 17 }; //1
                    grid_point_[2] = new int[2] { 7, 7 }; //2
                    grid_point_[3] = new int[2] { 7, 17 }; //3

                    grid_point_[4] = new int[2] { 7, 0 }; //4
                    grid_point_[5] = new int[2] { 7, 24 }; //5
                    grid_point_[6] = new int[2] { 17, 0 }; //6
                    grid_point_[7] = new int[2] { 17, 24 }; //7

                    grid_point_[8] = new int[2] { 17, 7 }; //8
                    grid_point_[9] = new int[2] { 17, 17 }; //9
                    grid_point_[10] = new int[2] { 24, 7 }; //10
                    grid_point_[11] = new int[2] { 24, 17 }; //11

                    texture_ = Resources.Load<Texture2D>("table/table_8_grid"); //テクスチャはそのうち変える
                    Debug.Log(texture_);
                    uv_coordinate_ = new Vector2[vertices_number_];
                    uv_coordinate_[0] = new Vector2(0.0F, 0.292F); //0
                    uv_coordinate_[1] = new Vector2(0.0F, 0.708F); //1
                    uv_coordinate_[2] = new Vector2(0.292F, 0.292F); //2
                    uv_coordinate_[3] = new Vector2(0.292F, 0.708F); //3

                    uv_coordinate_[4] = new Vector2(0.292F, 0.0F); //4
                    uv_coordinate_[5] = new Vector2(0.292F, 1.0F); //5
                    uv_coordinate_[6] = new Vector2(0.708F, 0.0F); //6
                    uv_coordinate_[7] = new Vector2(0.708F, 1.0F); //7

                    uv_coordinate_[8] = new Vector2(0.708F, 0.292F); //8
                    uv_coordinate_[9] = new Vector2(0.708F, 0.708F); //9
                    uv_coordinate_[10] = new Vector2(1.0F, 0.292F); //10
                    uv_coordinate_[11] = new Vector2(1.0F, 0.708F); //11

                    children_grid_ = new GameObject[children_number_];
                    //頂点インデックス生成
                    triangles_ = new int[children_number_][];
                    triangles_[0] = new int[4] { 0, 1, 2, 3 };
                    triangles_[1] = new int[4] { 4, 5, 6, 7 };
                    triangles_[2] = new int[4] { 8, 9, 10, 11 };

                    //枠線
                    outline_index_ = new int[24] { 0, 2, 2, 4, 4, 6, 6, 8, 8, 10, 10, 11, 11, 9, 9, 7, 7, 5, 5, 3, 3, 1, 1, 0 };
                    blueflag_index_ = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

                    //パラメータの設定
                    parameta_[0] = 0; //ダミー

                    //-------------------------------------------------

                    color_name_.Add(ColorName.White);
                    color_name_weight_.Add(10);

                    color_name_.Add(ColorName.Gray);
                    color_name_weight_.Add(5);

                    //-------------------------------------------------

                    material_type_.Add(MaterialType.Glass);
                    material_type_weight_.Add(6);

                    material_type_.Add(MaterialType.Metal);
                    material_type_weight_.Add(5);

                    material_type_.Add(MaterialType.NaturalFibre);
                    material_type_weight_.Add(4);


                    //-------------------------------------------------

                    pattern_type_.Add(PatternType.Check);
                    pattern_type_weight_.Add(6);

                    pattern_type_.Add(PatternType.Border);
                    pattern_type_weight_.Add(3);

                    //-------------------------------------------------

                    form_type_.Add(FormType.Rectangle);
                    form_type_weight_.Add(15);

                    //--------------------------------------------------

                    characteristic_.Add(Characteristic.Hard);
                    characteristic_weight_.Add(6);

                    //--------------------------------------------------

                    break;
                }

            case 9:
                {
                    //テーブルタイプ9

                    object_type_ = ObjectType.Normal;
                    children_number_ = 3;
                    center_point_ = new int[2] { 13, 13 }; //中心のグリッド座標
                    put_point_ = new int[2] { 13, 13 }; //上に乗る家具の中心が合わせる座標
                                                        //使用する頂点グリッド
                    vertices_number_ = 12;
                    grid_point_ = new int[vertices_number_][];
                    grid_point_[0] = new int[2] { 0, 8 }; //0
                    grid_point_[1] = new int[2] { 0, 18 }; //1
                    grid_point_[2] = new int[2] { 8, 8 }; //2
                    grid_point_[3] = new int[2] { 8, 18 }; //3

                    grid_point_[4] = new int[2] { 8, 0 }; //4
                    grid_point_[5] = new int[2] { 8, 26 }; //5
                    grid_point_[6] = new int[2] { 18, 0 }; //6
                    grid_point_[7] = new int[2] { 18, 26 }; //7

                    grid_point_[8] = new int[2] { 18, 8 }; //8
                    grid_point_[9] = new int[2] { 18, 18 }; //9
                    grid_point_[10] = new int[2] { 26, 8 }; //10
                    grid_point_[11] = new int[2] { 26, 18 }; //11

                    texture_ = Resources.Load<Texture2D>("table/table_9_grid"); //テクスチャはそのうち変える
                    Debug.Log(texture_);
                    uv_coordinate_ = new Vector2[vertices_number_];
                    uv_coordinate_[0] = new Vector2(0.0F, 0.308F); //0
                    uv_coordinate_[1] = new Vector2(0.0F, 0.692F); //1
                    uv_coordinate_[2] = new Vector2(0.308F, 0.308F); //2
                    uv_coordinate_[3] = new Vector2(0.308F, 0.692F); //3

                    uv_coordinate_[4] = new Vector2(0.308F, 0.0F); //4
                    uv_coordinate_[5] = new Vector2(0.308F, 1.0F); //5
                    uv_coordinate_[6] = new Vector2(0.692F, 0.0F); //6
                    uv_coordinate_[7] = new Vector2(0.692F, 1.0F); //7

                    uv_coordinate_[8] = new Vector2(0.692F, 0.308F); //8
                    uv_coordinate_[9] = new Vector2(0.692F, 0.692F); //9
                    uv_coordinate_[10] = new Vector2(1.0F, 0.308F); //10
                    uv_coordinate_[11] = new Vector2(1.0F, 0.692F); //11

                    children_grid_ = new GameObject[children_number_];
                    //頂点インデックス生成
                    triangles_ = new int[children_number_][];
                    triangles_[0] = new int[4] { 0, 1, 2, 3 };
                    triangles_[1] = new int[4] { 4, 5, 6, 7 };
                    triangles_[2] = new int[4] { 8, 9, 10, 11 };

                    //枠線
                    outline_index_ = new int[24] { 0, 2, 2, 4, 4, 6, 6, 8, 8, 10, 10, 11, 11, 9, 9, 7, 7, 5, 5, 3, 3, 1, 1, 0 };
                    blueflag_index_ = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

                    //パラメータの設定
                    parameta_[0] = 0; //ダミー

                    //-------------------------------------------------

                    color_name_.Add(ColorName.Beige);
                    color_name_weight_.Add(5);

                    color_name_.Add(ColorName.Cream);
                    color_name_weight_.Add(5);

                    color_name_.Add(ColorName.White);
                    color_name_weight_.Add(5);

                    color_name_.Add(ColorName.Brown);
                    color_name_weight_.Add(1);

                    color_name_.Add(ColorName.Orange);
                    color_name_weight_.Add(1);

                    //-------------------------------------------------

                    material_type_.Add(MaterialType.Marble);
                    material_type_weight_.Add(8);

                    material_type_.Add(MaterialType.NaturalFibre);
                    material_type_weight_.Add(7);

                    material_type_.Add(MaterialType.Ceramic);
                    material_type_weight_.Add(2);

                    material_type_.Add(MaterialType.Water);
                    material_type_weight_.Add(2);

                    //-------------------------------------------------

                    //-------------------------------------------------

                    form_type_.Add(FormType.Round);
                    form_type_weight_.Add(15);

                    //--------------------------------------------------

                    characteristic_.Add(Characteristic.Hard);
                    characteristic_weight_.Add(6);

                    characteristic_.Add(Characteristic.Luxury);
                    characteristic_weight_.Add(4);

                    //--------------------------------------------------

                    break;
                }

            case 10:
                {
                    //テーブルタイプ10

                    object_type_ = ObjectType.Normal;
                    children_number_ = 3;
                    center_point_ = new int[2] { 12, 10 }; //中心のグリッド座標
                    put_point_ = new int[2] { 12, 10 }; //上に乗る家具の中心が合わせる座標
                                                        //使用する頂点グリッド
                    vertices_number_ = 12;
                    grid_point_ = new int[vertices_number_][];
                    grid_point_[0] = new int[2] { 0, 6 }; //0
                    grid_point_[1] = new int[2] { 0, 15 }; //1
                    grid_point_[2] = new int[2] { 4, 6 }; //2
                    grid_point_[3] = new int[2] { 4, 15 }; //3

                    grid_point_[4] = new int[2] { 4, 0 }; //4
                    grid_point_[5] = new int[2] { 4, 21 }; //5
                    grid_point_[6] = new int[2] { 20, 0 }; //6
                    grid_point_[7] = new int[2] { 20, 21 }; //7

                    grid_point_[8] = new int[2] { 20, 6 }; //8
                    grid_point_[9] = new int[2] { 20, 15 }; //9
                    grid_point_[10] = new int[2] { 24, 6 }; //10
                    grid_point_[11] = new int[2] { 24, 15 }; //11

                    texture_ = Resources.Load<Texture2D>("table/table_10_grid"); //テクスチャはそのうち変える
                    Debug.Log(texture_);
                    uv_coordinate_ = new Vector2[vertices_number_];
                    uv_coordinate_[0] = new Vector2(0.0F, 0.286F); //0
                    uv_coordinate_[1] = new Vector2(0.0F, 0.714F); //1
                    uv_coordinate_[2] = new Vector2(0.167F, 0.286F); //2
                    uv_coordinate_[3] = new Vector2(0.167F, 0.714F); //3

                    uv_coordinate_[4] = new Vector2(0.167F, 0.0F); //4
                    uv_coordinate_[5] = new Vector2(0.167F, 1.0F); //5
                    uv_coordinate_[6] = new Vector2(0.833F, 0.0F); //6
                    uv_coordinate_[7] = new Vector2(0.833F, 1.0F); //7

                    uv_coordinate_[8] = new Vector2(0.833F, 0.286F); //8
                    uv_coordinate_[9] = new Vector2(0.833F, 0.714F); //9
                    uv_coordinate_[10] = new Vector2(1.0F, 0.286F); //10
                    uv_coordinate_[11] = new Vector2(1.0F, 0.714F); //11

                    children_grid_ = new GameObject[children_number_];
                    //頂点インデックス生成
                    triangles_ = new int[children_number_][];
                    triangles_[0] = new int[4] { 0, 1, 2, 3 };
                    triangles_[1] = new int[4] { 4, 5, 6, 7 };
                    triangles_[2] = new int[4] { 8, 9, 10, 11 };

                    //枠線
                    outline_index_ = new int[24] { 0, 2, 2, 4, 4, 6, 6, 8, 8, 10, 10, 11, 11, 9, 9, 7, 7, 5, 5, 3, 3, 1, 1, 0 };
                    blueflag_index_ = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

                    //パラメータの設定
                    parameta_[0] = 0; //ダミー

                    //-------------------------------------------------

                    color_name_.Add(ColorName.White);
                    color_name_weight_.Add(11);

                    color_name_.Add(ColorName.Black);
                    color_name_weight_.Add(4);

                    color_name_.Add(ColorName.LightBlue);
                    color_name_weight_.Add(2);

                    color_name_.Add(ColorName.Brown);
                    color_name_weight_.Add(1);

                    color_name_.Add(ColorName.Red);
                    color_name_weight_.Add(1);

                    color_name_.Add(ColorName.Yellow);
                    color_name_weight_.Add(1);

                    //-------------------------------------------------

                    material_type_.Add(MaterialType.Marble);
                    material_type_weight_.Add(9);

                    material_type_.Add(MaterialType.NaturalFibre);
                    material_type_weight_.Add(6);

                    material_type_.Add(MaterialType.Ceramic);
                    material_type_weight_.Add(2);

                    material_type_.Add(MaterialType.Glass);
                    material_type_weight_.Add(2);

                    material_type_.Add(MaterialType.Water);
                    material_type_weight_.Add(1);

                    material_type_.Add(MaterialType.ArtificialFoliage);
                    material_type_weight_.Add(1);


                    //-------------------------------------------------

                    pattern_type_.Add(PatternType.Flower);
                    pattern_type_weight_.Add(3);

                    pattern_type_.Add(PatternType.Arch);
                    pattern_type_weight_.Add(3);

                    //-------------------------------------------------

                    form_type_.Add(FormType.Rectangle);
                    form_type_weight_.Add(15);

                    //--------------------------------------------------

                    characteristic_.Add(Characteristic.Luxury);
                    characteristic_weight_.Add(4);

                    //--------------------------------------------------

                    break;
                }
        }
    }
}