using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LoginApp1.Models
{
    public class ModelUpdates
    {
        public string field_name { get; set; }
        public object old_value  { get; set; }
        public object new_value { get; set; }
        public override string ToString()
        { return field_name + ";" + old_value.ToString() + ";" + new_value.ToString(); }
    }

    public class GetModelUpdates
    {

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        // overloads that will let us call the update function with different parameters
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<ModelUpdates> GetUpdates<T>(T form_model, T db_model) where T : class
        { return GetUpdates(form_model, db_model, null, null); }

        public static List<ModelUpdates> GetUpdates<T>(T form_model, T db_model, List<string> exclude_flds) where T : class
        { return GetUpdates(form_model, db_model, exclude_flds, null); }


        public static List<ModelUpdates> GetUpdates<T>(T view_model, T db_model, List<string> exclude_flds, List<string> include_flds) where T : class
        {
            if (exclude_flds == null) { exclude_flds = new List<string>(); }
            if (include_flds == null) { include_flds = new List<string>(); }

            List<ModelUpdates> rtn_list   = new List<ModelUpdates>();
            List<PropertyInfo> form_props = view_model.GetType().GetProperties().OrderBy(o => o.Name).ToList();
            List<PropertyInfo> db_props   = db_model.GetType().GetProperties().OrderBy(o => o.Name).ToList();

            foreach (PropertyInfo f_prop in form_props)
            {
                PropertyInfo d_prop = db_props.Find(f => f.Name == f_prop.Name);

                if (f_prop.GetMethod == null)           { continue; }
                if (exclude_flds.Contains(f_prop.Name)) { continue; }
                if (!(include_flds.Count() == 0 || include_flds.Contains(f_prop.Name))) { continue; }

                object f_obj = f_prop.GetValue(view_model, null);
                object d_obj = d_prop.GetValue(db_model, null);

                if (f_obj == null && d_obj == null) { continue; }

                if (f_obj != null && d_obj == null)
                {
                    d_prop.SetValue(db_model, f_obj);
                    UpdateRtnList(f_prop.Name, f_obj, d_obj, rtn_list);
                }

                else if (f_obj == null && d_obj != null)
                {
                    d_prop.SetValue(db_model, f_obj);
                    UpdateRtnList(f_prop.Name, f_obj, d_obj, rtn_list);
                }

                else if (f_obj.ToString() != d_obj.ToString())
                {
                    d_prop.SetValue(db_model, f_obj);
                    UpdateRtnList(f_prop.Name, f_obj, d_obj, rtn_list);
                }

            }
            return rtn_list;
        }

        private static void UpdateRtnList(string prop_name, object f_obj, object d_obj, List<ModelUpdates> rtn_list)
        {
            ModelUpdates tmp_update = new ModelUpdates();
            tmp_update.field_name = prop_name;
            tmp_update.old_value = d_obj;
            tmp_update.new_value = f_obj;
            rtn_list.Add(tmp_update);
        }


    }
}