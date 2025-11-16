using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MilkTeaShop.DAL.Reposotories
{
    public class TableRepo
    {
        // Lấy tất cả bàn
        public List<Table> GetAll()
        {
            using var ctx = new MilkTeaShopDbContext();
            return ctx.Tables.ToList();
        }

        // Lấy bàn theo ID
        public Table? GetById(int tableId)
        {
            using var ctx = new MilkTeaShopDbContext();
            return ctx.Tables.FirstOrDefault(t => t.TableId == tableId);
        }

        // Cập nhật trạng thái bàn
        public void UpdateStatus(int tableId, string status)
        {
            using var ctx = new MilkTeaShopDbContext();
            var table = ctx.Tables.FirstOrDefault(t => t.TableId == tableId);
            if (table != null)
            {
                table.Status = status;
                ctx.SaveChanges();
            }
        }
    }
}
