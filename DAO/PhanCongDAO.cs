﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class PhanCongDAO
    {
        public int xoaPhanCong2(string idHopDong)
        {
            int flag = 0;
            Connection connect = new Connection();
            MySqlConnection con = connect.getConnection();
            try
            {
                con.Open();
                using (MySqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        // Lấy danh sách idChiTiet từ bảng chitiethopdong
                        List<int> chiTietIds = new List<int>();
                        string getChiTietIdsQuery = "SELECT idChiTiet FROM chitiethopdong WHERE idHopDong = @idHopDong";
                        using (MySqlCommand cmd = new MySqlCommand(getChiTietIdsQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@idHopDong", Int32.Parse(idHopDong));
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    chiTietIds.Add(reader.GetInt32("idChiTiet"));
                                }
                            }
                        }

                        // Duyệt từng phần tử trong danh sách idChiTiet
                        foreach (int idChiTiet in chiTietIds)
                        {
                            // Xóa các record trong bảng phancongduadon có idChiTiet trùng với phần tử được duyệt
                            string deletePhanCongQuery = "DELETE FROM phancongduadon WHERE idChiTiet = @idChiTiet";
                            using (MySqlCommand cmd = new MySqlCommand(deletePhanCongQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@idChiTiet", idChiTiet);
                                cmd.ExecuteNonQuery();
                            }

                            // Xóa idChiTiet trong bảng chitiethopdong
                            string deleteChiTietQuery = "DELETE FROM chitiethopdong WHERE idChiTiet = @idChiTiet";
                            using (MySqlCommand cmd = new MySqlCommand(deleteChiTietQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@idChiTiet", idChiTiet);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Cập nhật lại trạng thái của hopdong
                        string updateHopDongStatusQuery = "UPDATE hopdong SET idTrangThai = 6, ngayCapNhatTrangThai = NOW() WHERE idHopDong = @idHopDong";
                        using (MySqlCommand cmd = new MySqlCommand(updateHopDongStatusQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@idHopDong", idHopDong);
                            cmd.ExecuteNonQuery();
                        }
                        flag++;
                        // Commit transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if any error occurs
                        transaction.Rollback();
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
                con.Close();
                return flag;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;

            }

        }
        public int getIDTuyenDuong(string TruongDen, string PhuongDon)
        {
            Connection connect = new Connection();
            MySqlConnection con = connect.getConnection();
            try
            {
                con.Open();
                string sql = "SELECT idTuyenDuong FROM tuyenduong WHERE TenTruongDen = @TenTruongDen AND PhuongDon = @PhuongDon";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@TenTruongDen", TruongDen);
                cmd.Parameters.AddWithValue("@PhuongDon", PhuongDon);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader reader = cmd.ExecuteReader();
                int idTuyenDuong = -1;
                if (reader.Read())
                {
                    idTuyenDuong = reader.GetInt32("idTuyenDuong");
                }
                con.Close();
                return idTuyenDuong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;

            }
        }
        public DataTable doDuLieuChuaPhanCong()
        {
            Connection connect = new Connection();
            MySqlConnection con = connect.getConnection();
            try
            {
                con.Open();
                string sql = "SELECT hd.idHopDong as 'ID hợp đồng', hd.NgayBatDauHopDong as 'Ngày bắt đầu', hd.NgayKetThucHopDong as 'Ngày kết thúc', cthd.idChiTiet as 'ID chi tiết hợp đồng', cthd.ThuTrongTuan as 'Thứ', cthd.Buoi as 'Buổi', ttpd.Phuong as 'Phường đón', tttd.TenTruong as 'Trường đến' FROM hopdong AS hd JOIN chitiethopdong AS cthd ON hd.idHopDong = cthd.idHopDong JOIN thongtindiadiemtram AS ttpd ON hd.idTramDon = ttpd.idTram JOIN thongtindiadiemtruong AS tttd ON hd.idTruong = tttd.idTruong LEFT JOIN phancongduadon pc ON cthd.idChiTiet = pc.idChiTiet WHERE pc.idChiTiet IS NULL;";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
        }
        public int tongGheByIdChuyen_NgayDuaDon(string idChuyen, string NgayDuaDon)
        {
            Connection connect = new Connection();
            MySqlConnection con = connect.getConnection();
            try
            {
                con.Open();
                string sql = "SELECT COUNT(*) FROM phancongduadon WHERE idChuyen = @idChuyen AND NgayDuaDon = @NgayDuaDon";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@idChuyen", Int32.Parse(idChuyen));
                cmd.Parameters.AddWithValue("@NgayDuaDon", NgayDuaDon);
                MySqlDataReader reader = cmd.ExecuteReader();
                int count = -1;
                if (reader.Read())
                {
                    count = reader.GetInt32("COUNT(*)");
                }
                con.Close();
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;

            }
        }
        public int suaPhanCong(string idChuyen, string ngayDuaDon, string idPhanCong)
        {
            Connection connect = new Connection();
            MySqlConnection con = connect.getConnection();
            try
            {
                con.Open();
                string sql1 = "UPDATE phancongduadon SET idChuyen = @idChuyen, NgayDuaDon = @NgayDuaDon WHERE idPhanCong = @idPhanCong;";
                MySqlCommand cmd = new MySqlCommand(sql1, con);
                cmd.Parameters.AddWithValue("@idChuyen", Int32.Parse(idChuyen));
                cmd.Parameters.AddWithValue("@NgayDuaDon", ngayDuaDon);
                cmd.Parameters.AddWithValue("@idPhanCong", Int32.Parse(idPhanCong));
                int rs1 = cmd.ExecuteNonQuery();
                con.Close();
                return rs1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public int themPhanCong(string idChiTiet, string idChuyen, string ngayDuaDon)
        {
            Connection connect = new Connection();
            MySqlConnection con = connect.getConnection();
            try
            {
                con.Open();
                string sql1 = "INSERT INTO phancongduadon (idChiTiet, idChuyen, NgayDuaDon) VALUES (@idChiTiet, @idChuyen, @NgayDuaDon);";
                MySqlCommand cmd = new MySqlCommand(sql1, con);
                cmd.Parameters.AddWithValue("@idChiTiet", Int32.Parse(idChiTiet));
                cmd.Parameters.AddWithValue("@idChuyen", Int32.Parse(idChuyen));
                cmd.Parameters.AddWithValue("@NgayDuaDon", ngayDuaDon);
                int rs1 = cmd.ExecuteNonQuery();
                con.Close();
                return rs1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public int xoaPhanCong(string idPhanCong)
        {
            Connection connect = new Connection();
            MySqlConnection con = connect.getConnection();
            try
            {
                con.Open();
                string sql = "DELETE FROM phancongduadon WHERE idPhanCong = @idPhanCong;";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@idPhanCong", Int32.Parse(idPhanCong));
                int rs = cmd.ExecuteNonQuery();
                con.Close();
                return rs;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public DataTable doDuLieu()
        {
            Connection connect = new Connection();
            MySqlConnection con = connect.getConnection();
            try
            {
                con.Open();
                string sql = "SELECT pcdd.idPhanCong as 'ID phân công', pcdd.idChiTiet as 'ID chi tiết hợp đồng', cthd.ThuTrongTuan as 'Thứ', cthd.Buoi as 'Buổi', ttpd.Phuong as 'Phường đón', tttd.TenTruong as 'Trường đến', pcdd.idChuyen as 'ID chuyến', pcdd.NgayDuaDon as 'Ngày đưa đón' FROM phancongduadon AS pcdd JOIN chitiethopdong AS cthd ON pcdd.idChiTiet = cthd.idChiTiet JOIN hopdong AS hd ON cthd.idHopDong = hd.idHopDong JOIN thongtindiadiemtram AS ttpd ON hd.idTramDon = ttpd.idTram JOIN thongtindiadiemtruong AS tttd ON hd.idTruong = tttd.idTruong WHERE pcdd.idChuyen IS NOT NULL AND pcdd.NgayDuaDon IS NOT NULL ORDER BY pcdd.NgayDuaDon ASC;";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
        }

    }
}
