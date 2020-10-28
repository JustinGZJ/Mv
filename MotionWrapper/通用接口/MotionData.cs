using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;


namespace MotionWrapper
{
    /// <summary>
    /// 所有控制器都应该具备的数据
    /// </summary>
    public class CMotionData
    {
        public const int maxDiNum = 1024, maxDoNum = 1024, maxAxisNum = 32;
        //数据区域 尽量做到16个一组 
        public volatile bool[] mdis = new bool[maxDiNum];
        public volatile bool[] mdos = new bool[maxDoNum];
        public volatile AxisRef[] axisRefs = new AxisRef[maxAxisNum];
        public CMotionData()
        {
            for (int i = 0; i < maxAxisNum; i++)
            {
                axisRefs[i] = new AxisRef("");
            }
        }
    }
    /// <summary>
    /// IO的总列表
    /// </summary>
    public static class CIoPrms
    {
        public static bool readok = false;//读取成功
        public static List<CInputOutputPrm> i = new List<CInputOutputPrm>();
        public static List<CInputOutputPrm> o = new List<CInputOutputPrm>();
        /// <summary>
        /// 从XML中读取轴的配置
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static bool Read(string xml = @"Confige/IoS.xml")
        {
            try
            {
                //解析XML文件
                XmlDocument doc = new XmlDocument();
                doc.Load(xml);
                if (doc != null)
                {
                    XmlElement root = doc.DocumentElement;
                    if (root != null && root.Name == "IOS")
                    {
                        //刀库状态
                        XmlNodeList nodes = root.SelectNodes("/IOS/INPUT/IO");
                        if (nodes != null)
                        {
                            for (int i = 0; i < nodes.Count; i++)
                            {
                                //先转换 如果成功再加入
                                CInputOutputPrm tmpPrm = new CInputOutputPrm();
                                try
                                {
                                    tmpPrm.name = nodes[i].InnerText;
                                    if (nodes[i].Attributes["ID"] != null)
                                    {
                                        tmpPrm.ID = nodes[i].Attributes["ID"].InnerText;
                                    }
                                    if (nodes[i].Attributes["Pin"] != null)
                                    {
                                        tmpPrm.pinStr = nodes[i].Attributes["Pin"].InnerText;
                                    }
                                    if (nodes[i].Attributes["ModeStr"] != null)
                                    {
                                        tmpPrm.modelStr = nodes[i].Attributes["ModeStr"].InnerText;
                                    } 
                                    if (nodes[i].Attributes["OftenOpen"].InnerText == "Yes" || nodes[i].Attributes["OftenOpen"].InnerText == "yes" || nodes[i].Attributes["OftenOpen"].InnerText == "YES")
                                    {
                                        tmpPrm.oftenOpen = true;
                                    }
                                    else
                                    {
                                        tmpPrm.oftenOpen = false;
                                    }
                                    if (nodes[i].Attributes["Type"] != null)
                                    {
                                        tmpPrm.ioType = (EIoType)short.Parse(nodes[i].Attributes["Type"].InnerText);
                                    }
                                    if (nodes[i].Attributes["Index"] != null)
                                    {
                                        tmpPrm.index = (short)int.Parse(nodes[i].Attributes["Index"].InnerText);
                                    }
                                    if (nodes[i].Attributes["Model"] != null)
                                    {
                                        tmpPrm.model = (short)int.Parse(nodes[i].Attributes["Model"].InnerText);
                                    }
                                    
                                    CIoPrms.i.Add(tmpPrm);
                                }
                                catch (Exception)
                                {
                                    CIoPrms.i.Clear();
                                    break;
                                }
                            }
                        }
                        nodes = root.SelectNodes("/IOS/OUTPUT/IO");
                        if (nodes != null)
                        {
                            for (int i = 0; i < nodes.Count; i++)
                            {
                                //先转换 如果成功再加入
                                CInputOutputPrm tmpPrm = new CInputOutputPrm();
                                try
                                {
                                    if (nodes[i].Attributes["ID"] != null)
                                    {
                                        tmpPrm.ID = nodes[i].Attributes["ID"].InnerText;
                                    }
                                    if (nodes[i].Attributes["Pin"] != null)
                                    {
                                        tmpPrm.pinStr = nodes[i].Attributes["Pin"].InnerText;
                                    }
                                    if (nodes[i].Attributes["ModeStr"] != null)
                                    {
                                        tmpPrm.modelStr = nodes[i].Attributes["ModeStr"].InnerText;
                                    }
                                    if (nodes[i].Attributes["OftenOpen"].InnerText == "Yes" || nodes[i].Attributes["OftenOpen"].InnerText == "yes" || nodes[i].Attributes["OftenOpen"].InnerText == "YES")
                                    {
                                        tmpPrm.oftenOpen = true;
                                    }
                                    else
                                    {
                                        tmpPrm.oftenOpen = false;
                                    }
                                    if (nodes[i].Attributes["Type"] != null)
                                    {
                                        tmpPrm.ioType = (EIoType)short.Parse(nodes[i].Attributes["Type"].InnerText);
                                    }
                                    if (nodes[i].Attributes["Index"] != null)
                                    {
                                        tmpPrm.index = (short)int.Parse(nodes[i].Attributes["Index"].InnerText);
                                    }
                                    if (nodes[i].Attributes["Model"] != null)
                                    {
                                        tmpPrm.model = (short)int.Parse(nodes[i].Attributes["Model"].InnerText);
                                    }
                                    CIoPrms.o.Add(tmpPrm);
                                }
                                catch (Exception)
                                {
                                    o.Clear();
                                    break;
                                }
                            }
                        }
                    }
                }
                if (i.Count == 0 || o.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("打开IO配置表失败");
                    return readok = false;
                }
                return readok = true;
            }
            catch (Exception)
            {
                i.Clear();
                o.Clear();
                System.Windows.Forms.MessageBox.Show("打开IO配置表失败");
                return readok = false;
            }
        }
        /// <summary>
        /// 推送到UI的datagrid
        /// </summary>
        /// <returns></returns>
        public static bool PushToUi_Status_Input(DataGridView view, Image defaultimg)
        {
            //轴的界面初始化
            try
            {
                view.Rows.Clear();
                view.AllowUserToAddRows = false;
                int line = 0;
                for (int index = 0; index < i.Count;)
                {
                    view.Rows.Add();
                    view.Rows[line].Cells[0].Value = (index + 1).ToString();
                    view.Rows[line].Cells[1].Value = i[index].modelStr;
                    view.Rows[line].Cells[2].Value = i[index].pinStr;
                    view.Rows[line].Cells[3].Value = i[index].name;
                    view.Rows[line].Cells[4].Value = defaultimg;
                    index++;
                    if (index < i.Count)
                    {
                        view.Rows[line].Cells[5].Value = (index + 1).ToString();
                        view.Rows[line].Cells[6].Value = i[index].modelStr;
                        view.Rows[line].Cells[7].Value = i[index].pinStr;
                        view.Rows[line].Cells[8].Value = i[index].name;
                        view.Rows[line].Cells[9].Value = defaultimg;
                        index++;
                    }
                    line++;
                }
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("UI中初始化输入映射表失败");
                return false;
            }
        }
        /// <summary>
        /// 从界面的列表中拉数据
        /// </summary>
        /// <returns></returns>
        public static bool PullFromUi_Status_Input()
        {
            return true;
        }
        /// <summary>
        /// 推送到UI的datagrid
        /// </summary>
        /// <returns></returns>
        public static bool PushToUi_Status_Output(DataGridView view, Image defaultimg)
        {
            //轴的界面初始化
            try
            {
                view.Rows.Clear();
                view.AllowUserToAddRows = false;
                int line = 0;
                for (int index = 0; index < o.Count;)
                {
                    view.Rows.Add();
                    view.Rows[line].Cells[0].Value = (index + 1).ToString();
                    view.Rows[line].Cells[1].Value = o[index].modelStr;
                    view.Rows[line].Cells[2].Value = o[index].pinStr;
                    view.Rows[line].Cells[3].Value = o[index].name;
                    view.Rows[line].Cells[4].Value = defaultimg;
                    index++;
                    if (index < o.Count)
                    {
                        view.Rows[line].Cells[5].Value = (index + 1).ToString();
                        view.Rows[line].Cells[6].Value = o[index].modelStr;
                        view.Rows[line].Cells[7].Value = o[index].pinStr;
                        view.Rows[line].Cells[8].Value = o[index].name;
                        view.Rows[line].Cells[9].Value = defaultimg;
                        index++;
                    }
                    line++;
                }
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("UI中初始化输出映射表失败");
                return false;
            }
        }
        /// <summary>
        /// 从界面的列表中拉数据
        /// </summary>
        /// <returns></returns>
        public static bool PullFromUi_Status_Output()
        {
            return true;
        }
    }

    /// <summary>
    /// 所有轴的参数
    /// </summary>
    public static class AxisParameters
    {
        public static bool readok = false;//读取成功
        public static List<AxisParameter> prms = new List<AxisParameter>();//原始数据中出现一个轴 就必须在这里有一个配置 
        public static List<CCrdPrm> crdS = new List<CCrdPrm>();
        /// <summary>
        /// 保存轴参数
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static bool Save(string xml = @"Confige/AxisS.xml")
        {
            try
            {
                //文档
                XmlWriterSettings set = new XmlWriterSettings();
                set.Indent = true;
                set.NewLineOnAttributes = false;
                XmlWriter writer = XmlWriter.Create(xml, set);
                //master
                writer.WriteStartDocument();

                writer.WriteStartElement("Motion");
                //crd
                writer.WriteStartElement("Crds");
                foreach (var item in crdS)
                {
                    writer.WriteStartElement("Crd");
                    writer.WriteAttributeString("CrdNum", item.crdNum.ToString());
                    writer.WriteAttributeString("CardNum", item.cardNum.ToString());
                    writer.WriteAttributeString("Dimension", item.dimension.ToString());
                    writer.WriteAttributeString("Pitch", item.Pith.ToString());
                    writer.WriteAttributeString("Resolution", item.Resolution.ToString());
                    writer.WriteAttributeString("SynVelMax", item.synVelMax.ToString());
                    writer.WriteAttributeString("SynAccMax", item.synAccMax.ToString());
                    writer.WriteAttributeString("X", item.x.ToString());
                    writer.WriteAttributeString("Y", item.y.ToString());
                    writer.WriteAttributeString("Z", item.z.ToString());
                    writer.WriteAttributeString("A", item.a.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();


                writer.WriteStartElement("AxisS");
                foreach (var item in prms)
                {
                    writer.WriteStartElement("Axis");
                    writer.WriteAttributeString("Name", item.Name);
                    writer.WriteAttributeString("AxisNum", item.AxisNum.ToString());
                    writer.WriteAttributeString("BackLash", item.BackLash.ToString());
                    writer.WriteAttributeString("CardNum", item.CardNum.ToString());
                    writer.WriteAttributeString("Resolution", item.Resolution.ToString());
                    writer.WriteAttributeString("Pitch", item.Pitch.ToString());
                    writer.WriteAttributeString("HomeLeave", item.HomeLeave.ToString());
                    writer.WriteAttributeString("Homeoffset", item.Homeoffset.ToString());
                    writer.WriteAttributeString("HomeSearch", item.HomeSearch.ToString());
                    writer.WriteAttributeString("HomeVel1", item.HomeVel1.ToString());
                    writer.WriteAttributeString("HomeVel2", item.HomeVel2.ToString());
                    writer.WriteAttributeString("MaxAcc", item.MaxAcc.ToString());
                    writer.WriteAttributeString("MaxVel", item.MaxVel.ToString());
                    writer.WriteAttributeString("Note", item.Note.ToString());
                    writer.WriteAttributeString("SmoothTime", item.SmoothTime.ToString());
                    writer.WriteAttributeString("HomeType", item.HomeType.ToString());
                    writer.WriteAttributeString("ArriveTime", item.ArriveDelay.ToString());
                    writer.WriteAttributeString("ArriveRange", item.ArrivePand.ToString());
                    writer.WriteAttributeString("ArriveType", item.ArriveType.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
                return true;
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("保存轴参数错误");
                return false;
            }
        }
        /// <summary>
        /// 从XML中读取轴的配置
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static bool Read(string xml = @"Confige/AxisS.xml")
        {
            try
            {
                //解析XML文件
                XmlDocument doc = new XmlDocument();
                doc.Load(xml);
                if (doc != null)
                {
                    XmlElement root = doc.DocumentElement;
                    if (root != null && root.Name == "Motion")
                    {
                        XmlNodeList nodes = root.SelectNodes("/Motion/AxisS/Axis");
                        if (nodes != null)
                        {
                            for (int i = 0; i < nodes.Count; i++)
                            {
                                //先转换 如果成功再加入
                                AxisParameter tmpPrm = new AxisParameter();

                                tmpPrm.CardNum = short.Parse(nodes[i].Attributes["CardNum"].InnerText);
                                tmpPrm.AxisNum = short.Parse(nodes[i].Attributes["AxisNum"].InnerText);

                                tmpPrm.Name = nodes[i].Attributes["AxisName"].InnerText;
                                //tmpPrm.ID = short.Parse(nodes[i].Attributes["ID"].InnerText);
                                tmpPrm.BackLash = float.Parse(nodes[i].Attributes["BackLash"].InnerText);
                                //tmpPrm.c = int.Parse(nodes[i].Attributes["cardnum"].InnerText);
                                tmpPrm.HomeSearch = float.Parse(nodes[i].Attributes["HomeS1"].InnerText);
                                tmpPrm.Resolution = float.Parse(nodes[i].Attributes["Resolution"].InnerText);
                                tmpPrm.Pitch = float.Parse(nodes[i].Attributes["Pitch"].InnerText);
                                tmpPrm.HomeLeave = float.Parse(nodes[i].Attributes["HomeLeave"].InnerText);
                                tmpPrm.Homeoffset = float.Parse(nodes[i].Attributes["HomeOffset"].InnerText);
                                //tmpPrm.homeoffset = double.Parse(nodes[i].Attributes["HomeS1"].InnerText);
                                tmpPrm.HomeVel1 = float.Parse(nodes[i].Attributes["HomeVel1"].InnerText);
                                tmpPrm.HomeVel2 = float.Parse(nodes[i].Attributes["HomeVel2"].InnerText);
                                tmpPrm.MaxAcc = float.Parse(nodes[i].Attributes["MaxAcc"].InnerText);
                                tmpPrm.MaxVel = float.Parse(nodes[i].Attributes["MaxVel"].InnerText);
                                tmpPrm.HomeTriger = short.Parse(nodes[i].Attributes["HomeTriger"].InnerText);
                                tmpPrm.Note = nodes[i].Attributes["Note"].InnerText;
                                //tmpPrm.sm = double.Parse(nodes[i].Attributes["Smoothtime"].InnerText);

                                tmpPrm.HomeType = short.Parse(nodes[i].Attributes["HomeType"].InnerText);
                                tmpPrm.ArriveDelay = short.Parse(nodes[i].Attributes["ArriveDelay"].InnerText);
                                tmpPrm.ArrivePand = float.Parse(nodes[i].Attributes["ArrivePand"].InnerText);
                                tmpPrm.ArriveType = short.Parse(nodes[i].Attributes["ArriveType"].InnerText);
                                prms.Add(tmpPrm);
                            }
                        }
                        //crd
                        nodes = root.SelectNodes("/Motion/Crds/Crd");
                        if (nodes != null)
                        {
                            for (int i = 0; i < nodes.Count; i++)
                            {
                                //先转换 如果成功再加入
                                CCrdPrm tmpPrm = new CCrdPrm();

                                tmpPrm.cardNum = int.Parse(nodes[i].Attributes["CardNum"].InnerText);
                                tmpPrm.crdNum = int.Parse(nodes[i].Attributes["CrdNum"].InnerText);
                                tmpPrm.dimension = int.Parse(nodes[i].Attributes["Dimension"].InnerText);
                                tmpPrm.Pith = int.Parse(nodes[i].Attributes["Pith"].InnerText);
                                tmpPrm.Resolution = double.Parse(nodes[i].Attributes["Resolution"].InnerText);
                                tmpPrm.synVelMax = double.Parse(nodes[i].Attributes["SynVelMax"].InnerText);
                                tmpPrm.synAccMax = double.Parse(nodes[i].Attributes["SynAccMax"].InnerText);
                                tmpPrm.x = int.Parse(nodes[i].Attributes["X"].InnerText);
                                tmpPrm.y = int.Parse(nodes[i].Attributes["Y"].InnerText);
                                tmpPrm.z = int.Parse(nodes[i].Attributes["Z"].InnerText);
                                tmpPrm.a = int.Parse(nodes[i].Attributes["A"].InnerText);
                                crdS.Add(tmpPrm);
                            }
                        }
                    }
                }
                return readok = true;
            }
            catch (Exception)
            {
                prms.Clear();
                crdS.Clear();
                return readok = false;
            }
        }
        /// <summary>
        /// 推送到状态栏
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        //public static bool PushToUi_Status(DataGridView view, Image defaultimg)
        //{
        //    //轴的界面初始化
        //    try
        //    {
        //        int index = 0;
        //        view.AllowUserToAddRows = false;
        //        foreach (var item in prms)
        //        {
        //            view.Rows.Add();
        //            view.Rows[index].Cells[0].Value = item.axisnum;
        //            view.Rows[index].Cells[1].Value = item.AxisName;
        //            view.Rows[index].Cells[2].Value = "0.00";
        //            view.Rows[index].Cells[3].Value = "0.00";
        //            view.Rows[index].Cells[4].Value = defaultimg;
        //            view.Rows[index].Cells[5].Value = defaultimg;
        //            view.Rows[index].Cells[6].Value = defaultimg;
        //            view.Rows[index].Cells[7].Value = defaultimg;
        //            view.Rows[index].Cells[8].Value = defaultimg;
        //            view.Rows[index].Cells[9].Value = item.note;
        //            index++;
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        /// <summary>
        /// 推送到状态栏
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static bool PullFromUi_Status(DataGridView view, Image defaultimg)
        {
            //轴的界面初始化
            return false;
        }
        /// <summary>
        /// 推送参数到UI
        /// </summary>
        /// <param name="view"></param>
        //public static void PushToUi_Prm(DataGridView view)
        //{
        //    if (view != null)
        //    {
        //        view.Rows.Clear();
        //        view.AllowUserToAddRows = false;
        //        int line = 0;
        //        foreach (var item in prms)
        //        {
        //            view.Rows.Add();
        //            view.Rows[line].Cells[0].Value = (line + 1).ToString();
        //            view.Rows[line].Cells[1].Value = item.Resolution.ToString();
        //            view.Rows[line].Cells[2].Value = item.Pith.ToString();
        //            view.Rows[line].Cells[3].Value = item.MaxVel.ToString();
        //            view.Rows[line].Cells[4].Value = item.MaxAcc.ToString();
        //            view.Rows[line].Cells[5].Value = item.HomeType.ToString();
        //            view.Rows[line].Cells[6].Value = item.HomeOffset.ToString();
        //            view.Rows[line].Cells[7].Value = item.HomeVel1.ToString();
        //            view.Rows[line].Cells[8].Value = item.ArriveType.ToString();
        //            view.Rows[line].Cells[9].Value = item.ArriveDelay.ToString();
        //            view.Rows[line].Cells[10].Value = item.ArrivePand.ToString();
        //            line++;
        //        }
        //    }
        //}
        /// <summary>
        /// 从UI中获取参数
        /// </summary>
        /// <param name="view"></param>
        //public static bool PullFromUi_Prm(DataGridView view)
        //{
        //    try
        //    {
        //        if (view != null && view.Rows.Count > 0)
        //        {
        //            AxisParameter checkprm = new AxisParameter();//先验证
        //            for (int i = 0; i < view.Rows.Count; i++)
        //            {
        //                try
        //                {
        //                    checkprm.Resolution = double.Parse((string)view.Rows[i].Cells[1].Value);
        //                    checkprm.Pith = double.Parse((string)view.Rows[i].Cells[2].Value);
        //                    checkprm.MaxVel = double.Parse((string)view.Rows[i].Cells[3].Value);
        //                    checkprm.MaxAcc = double.Parse((string)view.Rows[i].Cells[4].Value);
        //                    checkprm.HomeType = int.Parse((string)view.Rows[i].Cells[5].Value);
        //                    checkprm.HomeOffset = double.Parse((string)view.Rows[i].Cells[6].Value);
        //                    checkprm.HomeVel1 = double.Parse((string)view.Rows[i].Cells[7].Value);
        //                    checkprm.ArriveType = int.Parse((string)view.Rows[i].Cells[8].Value);
        //                    checkprm.ArriveDelay = int.Parse((string)view.Rows[i].Cells[9].Value);
        //                    checkprm.ArrivePand = double.Parse((string)view.Rows[i].Cells[10].Value);
        //                }
        //                catch (Exception)
        //                {
        //                    MessageBox.Show("检测参数错误");
        //                    return false;
        //                }
        //            }
        //            for (int i = 0; i < view.Rows.Count; i++)
        //            {
        //                prms[i].Resolution = double.Parse((string)view.Rows[i].Cells[1].Value);
        //                prms[i].Pith = double.Parse((string)view.Rows[i].Cells[2].Value);
        //                prms[i].MaxVel = double.Parse((string)view.Rows[i].Cells[3].Value);
        //                prms[i].MaxAcc = double.Parse((string)view.Rows[i].Cells[4].Value);
        //                prms[i].HomeType = int.Parse((string)view.Rows[i].Cells[5].Value);
        //                prms[i].HomeOffset = double.Parse((string)view.Rows[i].Cells[6].Value);
        //                prms[i].HomeVel1 = double.Parse((string)view.Rows[i].Cells[7].Value);
        //                prms[i].ArriveType = int.Parse((string)view.Rows[i].Cells[8].Value);
        //                prms[i].ArriveDelay = int.Parse((string)view.Rows[i].Cells[9].Value);
        //                prms[i].ArrivePand = double.Parse((string)view.Rows[i].Cells[10].Value);
        //            }
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("检测参数错误");
        //        return false;
        //    }
        //}
    }
}
