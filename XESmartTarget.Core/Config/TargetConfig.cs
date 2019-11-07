﻿using DouglasCrockford.JsMin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using XESmartTarget.Core.Utils;

namespace XESmartTarget.Core.Config
{
    public class TargetConfig
    {
        public Target Target { get; set; }


        public static void Test()
        {
            JavaScriptSerializer ser = new JavaScriptSerializer(new TargetConfigTypeResolver());
            ser.RegisterConverters(new JavaScriptConverter[] { new ModelConverter() });
            TargetConfig x = new TargetConfig()
            {
                Target = new Target()
            };
            x.Target.SessionName = "system_health";
            Responses.TableAppenderResponse res = new Responses.TableAppenderResponse()
            {
                UploadIntervalSeconds = 0,
                TableName = "someTable",
                Filter = "someField = \"SomeValue\""
            };
            res.Events.Add("SomeEvent");
            x.Target.Responses.Add(res);
            string s = ser.Serialize(x);

            TargetConfig tc = ser.Deserialize<TargetConfig>(Samples.Sample.ToString());
            
        }


        public static TargetConfig LoadFromFile(string path)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer(new TargetConfigTypeResolver());
            ser.RegisterConverters(new JavaScriptConverter[] { new ModelConverter() });
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                var minifier = new JsMinifier();
                // minify JSON to strip away comments
                // Comments in config files are very useful but JSON parsers
                // do not allow comments. Minification solves the issue.
                string jsonMin = minifier.Minify(json);
                return ser.Deserialize<TargetConfig>(jsonMin);
            }
        }

    }
}
