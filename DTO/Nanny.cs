﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Nanny
    {
        private int idbaomau;
        private string ho;
        private string ten;
        private string gioitinh;
        private string ngaysinh;
        private string cccd;
        private string email;
        private string sdt;
        private string pathimage;
        private string diachi;
        public Nanny() { }
        public Nanny(string ho, string ten, string gioitinh, string ngaysinh, string cccd, string email, string sdt, string diachi, string pathimage)
        {

            this.ho = ho;
            this.ten = ten;
            this.gioitinh = gioitinh;
            this.ngaysinh = ngaysinh;
            this.cccd = cccd;
            this.email = email;
            this.sdt = sdt;
            this.pathimage = pathimage;
            this.diachi = diachi;
        }

        public int Idbaomau { get => idbaomau; set => idbaomau = value; }
        public string Ho { get => ho; set => ho = value; }
        public string Ten { get => ten; set => ten = value; }
        public string Gioitinh { get => gioitinh; set => gioitinh = value; }
        public string Ngaysinh { get => ngaysinh; set => ngaysinh = value; }
        public string Cccd { get => cccd; set => cccd = value; }
        public string Email { get => email; set => email = value; }
        public string Sdt { get => sdt; set => sdt = value; }
        public string Pathimage { get => pathimage; set => pathimage = value; }
        public string Diachi { get => diachi; set => diachi = value; }
    }
}
