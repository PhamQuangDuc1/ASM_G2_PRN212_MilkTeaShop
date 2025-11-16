using MilkTeaShop.DAL.Entities;
using MilkTeaShop.DAL.Reposotories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.BLL.Services
{
    public class TablesServices
    {
        private TableRepo _repo = new();

        public List<Table> GetAllTables()
        {
            return _repo.GetAll();
        }

        // Lấy bàn theo ID
        public Table? GetTableById(int tableId)
        {
            return _repo.GetById(tableId);
        }

        // Cập nhật trạng thái bàn
        public void UpdateTableStatus(int tableId, string status)
        {
            _repo.UpdateStatus(tableId, status);
        }
    }
}
