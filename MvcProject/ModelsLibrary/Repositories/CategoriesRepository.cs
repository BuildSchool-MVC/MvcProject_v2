﻿using ModelsLibrary.DtO_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Abstracts;

namespace ModelsLibrary.Repositories
{
    public class CategoriesRepository : IRepository<Categories>
    {
        public void Create(Categories model)
        {
            SqlConnection connection = new SqlConnection(
                "data source=.; database=BuildSchool; integrated security=true");
            var sql = "INSERT INTO Categories VALUES (@Cid, @Cname)";

            connection.Execute(sql, new { Cid = model.CategoryID, Cname = model.CategoryName });
        }

        public void Delete(Categories model)
        {
            SqlConnection connection = new SqlConnection(
               "data source=.; database=BuildSchool; integrated security=true");
            var sql = "DELETE FROM Categories WHERE CategoryID = @Cid";

            connection.Execute(sql, new { Cid = model.CategoryID });
        }

        public void UpdateCategoryNameByID(int cid, string cname)
        {
            SqlConnection connection = new SqlConnection(
              "data source=.; database=BuildSchool; integrated security=true");
            var sql = "UPDATE Categories SET CategoryName = @inputCName WHERE CategoryID = @SearchCid";

            connection.Execute(sql, new { SearchCid = cid, inputCName = cname });
        }

        public Categories GetByID(int Cid)
        {
            SqlConnection connection = new SqlConnection(
                "data source=.; database=BuildSchool; integrated security=true");

            var list = connection.Query<Categories>("SELECT * FROM Categories WHERE CategoryID = @id"
                , new { id = Cid });

            Categories category = null;
            foreach (var item in list)
            {
                category = item;
            }

            return category;
        }

        public Categories GetByName(string CName)
        {
            SqlConnection connection = new SqlConnection(
                "data source=.; database=BuildSchool; integrated security=true");

            var list = connection.Query<Categories>("SELECT * FROM Categories WHERE CategoryName = @name"
                , new { name = CName });

            Categories category = null;
            foreach (var item in list)
            {
                category = item;
            }

            return category;
        }

        public IEnumerable<Categories> GetAll()
        {
            var connection = new SqlConnection("data source=.; database=BuildSchool; integrated security=true");
            return connection.Query<Categories>("SELECT * FROM Categories");
        }
    }
}
