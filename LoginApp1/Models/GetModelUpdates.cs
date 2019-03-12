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
    }

    public class GetModelUpdates
    {
        public static List<ModelUpdates> GetUpdates<T>(T form_model, T db_model, List<string> inc_flds) where T : class
        {
            List<ModelUpdates> rtn_list = new List<ModelUpdates>();

            List<PropertyInfo> form_props = form_model.GetType().GetProperties().OrderBy(o => o.Name).ToList();
            List<PropertyInfo> db_props = db_model.GetType().GetProperties().OrderBy(o => o.Name).ToList();

            foreach (PropertyInfo f_prop in form_props)
            {
                var f_obj = f_prop.GetValue(form_model, null);
                PropertyInfo d_prop = db_props.Find(f => f.Name == f_prop.Name);
                var d_obj = d_prop.GetValue(db_model, null);
                if (f_obj != null && d_obj != null)
                {
                    if (f_obj.ToString() != d_obj.ToString())
                    {
                        if (inc_flds.Contains(f_prop.Name))
                        {
                            ModelUpdates tmp_update = new ModelUpdates();
                            tmp_update.field_name = f_prop.Name;
                            tmp_update.old_value = d_obj.ToString();
                            tmp_update.new_value = f_obj.ToString();
                            rtn_list.Add(tmp_update);
                            d_prop.SetValue(db_model, f_obj);
                        }
                    }
                }
            }
            return rtn_list;
        }

        //public static Dictionary<String, object> GetUpdates<T>(T form_model, T db_model) where T : class
        //{
        //    Dictionary<String, object> rtn_dict = new Dictionary<string, object>();

        //    List<PropertyInfo> form_props = form_model.GetType().GetProperties().OrderBy(o=>o.Name).ToList();
        //    List<PropertyInfo> db_props = db_model.GetType().GetProperties().OrderBy(o => o.Name).ToList();

        //    foreach (PropertyInfo f_prop in form_props)
        //    {
        //        var f_obj = f_prop.GetValue(form_model, null);
        //        PropertyInfo d_prop = db_props.Find(f => f.Name == f_prop.Name);
        //        var d_obj = d_prop.GetValue(db_model, null);
        //        if (f_obj != null && d_obj != null)
        //        {
        //            if (f_obj.ToString() != d_obj.ToString())
        //            {
        //                rtn_dict.Add(f_prop.Name, f_obj);
        //            }
        //        }
        //    }
        //    return rtn_dict;
        //}

    }
}