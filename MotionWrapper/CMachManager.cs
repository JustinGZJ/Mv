using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace GoogolMotion
{
    /// <summary>
    /// 参数类
    /// </summary>
    public static class CPrms
    {
        public static bool Read()
        {
            return true;
        }
        public static bool Save()
        {
            return true;
        }
    }

    /// <summary>
    /// IO的总列表
    /// </summary>
    public static class CIoPrms
    {
        public static bool readok = false;//读取成功
        public static List<CIoPrm> i = new List<CIoPrm>();
        public static List<CIoPrm> o = new List<CIoPrm>();
        /// <summary>
        /// 从XML中读取轴的配置
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static bool Read(string xml = @"Confige/Ios.xml")
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
                                CIoPrm tmpPrm = new CIoPrm();
                                try
                                {
                                    tmpPrm.pin = nodes[i].Attributes["pin"].InnerText;
                                    tmpPrm.mode = nodes[i].Attributes["mode"].InnerText;
                                    tmpPrm.name = nodes[i].Attributes["name"].InnerText;
                                    if (nodes[i].Attributes["often_open"].InnerText == "Yes" || nodes[i].Attributes["often_open"].InnerText == "yes" || nodes[i].Attributes["often_open"].InnerText == "YES")
                                    {
                                        tmpPrm.often_open = true;
                                    }
                                    else
                                    {
                                        tmpPrm.often_open = false;
                                    }
                                    tmpPrm.index = int.Parse(nodes[i].Attributes["index"].InnerText);
                                    tmpPrm.modelNum = int.Parse(nodes[i].Attributes["type"].InnerText);
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
                                CIoPrm tmpPrm = new CIoPrm();
                                try
                                {
                                    tmpPrm.index = int.Parse(nodes[i].Attributes["index"].InnerText);
                                    tmpPrm.mode = nodes[i].Attributes["mode"].InnerText;
                                    tmpPrm.name = nodes[i].Attributes["name"].InnerText;
                                    if (nodes[i].Attributes["often_open"].InnerText == "Yes" || nodes[i].Attributes["often_open"].InnerText == "yes" || nodes[i].Attributes["often_open"].InnerText == "YES")
                                    {
                                        tmpPrm.often_open = true;
                                    }
                                    else
                                    {
                                        tmpPrm.often_open = false;
                                    }
                                    tmpPrm.pin = nodes[i].Attributes["pin"].InnerText;
                                    tmpPrm.modelNum = int.Parse(nodes[i].Attributes["type"].InnerText);
                                    o.Add(tmpPrm);
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
                    view.Rows[line].Cells[1].Value = i[index].mode;
                    view.Rows[line].Cells[2].Value = i[index].pin;
                    view.Rows[line].Cells[3].Value = i[index].name;
                    view.Rows[line].Cells[4].Value = defaultimg;
                    index++;
                    if (index < i.Count)
                    {
                        view.Rows[line].Cells[5].Value = (index + 1).ToString();
                        view.Rows[line].Cells[6].Value = i[index].mode;
                        view.Rows[line].Cells[7].Value = i[index].pin;
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
                    view.Rows[line].Cells[1].Value = o[index].mode;
                    view.Rows[line].Cells[2].Value = o[index].pin;
                    view.Rows[line].Cells[3].Value = o[index].name;
                    view.Rows[line].Cells[4].Value = defaultimg;
                    index++;
                    if (index < o.Count)
                    {
                        view.Rows[line].Cells[5].Value = (index + 1).ToString();
                        view.Rows[line].Cells[6].Value = o[index].mode;
                        view.Rows[line].Cells[7].Value = o[index].pin;
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
    public static class CAxisPrms
    {
        public static bool readok = false;//读取成功
        public static List<CAxisPrm> prms = new List<CAxisPrm>();//原始数据中出现一个轴 就必须在这里有一个配置 
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
                    writer.WriteAttributeString("crdNum", item.crdNum.ToString());
                    writer.WriteAttributeString("cardNum", item.cardNum.ToString());
                    writer.WriteAttributeString("dimension", item.dimension.ToString());
                    writer.WriteAttributeString("Pith", item.Pith.ToString());
                    writer.WriteAttributeString("Resolution", item.Resolution.ToString());
                    writer.WriteAttributeString("synVelMax", item.synVelMax.ToString());
                    writer.WriteAttributeString("synAccMax", item.synAccMax.ToString());
                    writer.WriteAttributeString("x", item.x.ToString());
                    writer.WriteAttributeString("y", item.y.ToString());
                    writer.WriteAttributeString("z", item.z.ToString());
                    writer.WriteAttributeString("a", item.a.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();


                writer.WriteStartElement("AxisS");
                foreach (var item in prms)
                {
                    writer.WriteStartElement("Axis");
                    writer.WriteAttributeString("AxisName", item.AxisName);
                    writer.WriteAttributeString("axisnum", item.axisnum.ToString());
                    writer.WriteAttributeString("BackLash", item.BackLash.ToString());
                    writer.WriteAttributeString("cardnum", item.cardnum.ToString());
                    writer.WriteAttributeString("Resolution", item.Resolution.ToString());
                    writer.WriteAttributeString("Pith", item.Pith.ToString());
                    writer.WriteAttributeString("HomeLeave", item.HomeLeave.ToString());
                    writer.WriteAttributeString("HomeOffset", item.HomeOffset.ToString());
                    writer.WriteAttributeString("HomeS1", item.HomeS1.ToString());
                    writer.WriteAttributeString("HomeVel1", item.HomeVel1.ToString());
                    writer.WriteAttributeString("HomeVel2", item.HomeVel2.ToString());
                    writer.WriteAttributeString("MaxAcc", item.MaxAcc.ToString());
                    writer.WriteAttributeString("MaxVel", item.MaxVel.ToString());
                    writer.WriteAttributeString("note", item.note.ToString());
                    writer.WriteAttributeString("Smoothtime", item.Smoothtime.ToString());
                    writer.WriteAttributeString("HomeType", item.HomeType.ToString());
                    writer.WriteAttributeString("ArriveDelay", item.ArriveDelay.ToString());
                    writer.WriteAttributeString("ArrivePand", item.ArrivePand.ToString());
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
                                CAxisPrm tmpPrm = new CAxisPrm();

                                tmpPrm.AxisName = nodes[i].Attributes["AxisName"].InnerText;
                                tmpPrm.axisnum = int.Parse(nodes[i].Attributes["axisnum"].InnerText);
                                tmpPrm.BackLash = double.Parse(nodes[i].Attributes["BackLash"].InnerText);
                                tmpPrm.cardnum = int.Parse(nodes[i].Attributes["cardnum"].InnerText);
                                tmpPrm.Resolution = double.Parse(nodes[i].Attributes["Resolution"].InnerText);
                                tmpPrm.Pith = double.Parse(nodes[i].Attributes["Pith"].InnerText);
                                tmpPrm.HomeLeave = double.Parse(nodes[i].Attributes["HomeLeave"].InnerText);
                                tmpPrm.HomeOffset = double.Parse(nodes[i].Attributes["HomeOffset"].InnerText);
                                tmpPrm.HomeS1 = double.Parse(nodes[i].Attributes["HomeS1"].InnerText);
                                tmpPrm.HomeVel1 = double.Parse(nodes[i].Attributes["HomeVel1"].InnerText);
                                tmpPrm.HomeVel2 = double.Parse(nodes[i].Attributes["HomeVel2"].InnerText);
                                tmpPrm.MaxAcc = double.Parse(nodes[i].Attributes["MaxAcc"].InnerText);
                                tmpPrm.MaxVel = double.Parse(nodes[i].Attributes["MaxVel"].InnerText);
                                tmpPrm.note = nodes[i].Attributes["note"].InnerText;
                                tmpPrm.Smoothtime = double.Parse(nodes[i].Attributes["Smoothtime"].InnerText);

                                tmpPrm.HomeType = int.Parse(nodes[i].Attributes["HomeType"].InnerText);
                                tmpPrm.ArriveDelay = int.Parse(nodes[i].Attributes["ArriveDelay"].InnerText);
                                tmpPrm.ArrivePand = double.Parse(nodes[i].Attributes["ArrivePand"].InnerText);
                                tmpPrm.ArriveType = int.Parse(nodes[i].Attributes["ArriveType"].InnerText);
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

                                tmpPrm.cardNum = int.Parse(nodes[i].Attributes["cardNum"].InnerText);
                                tmpPrm.crdNum = int.Parse(nodes[i].Attributes["crdNum"].InnerText);
                                tmpPrm.dimension = int.Parse(nodes[i].Attributes["dimension"].InnerText);
                                tmpPrm.Pith = int.Parse(nodes[i].Attributes["Pith"].InnerText);
                                tmpPrm.Resolution = double.Parse(nodes[i].Attributes["Resolution"].InnerText);
                                tmpPrm.synVelMax = double.Parse(nodes[i].Attributes["synVelMax"].InnerText);
                                tmpPrm.synAccMax = double.Parse(nodes[i].Attributes["synAccMax"].InnerText);
                                tmpPrm.x = int.Parse(nodes[i].Attributes["x"].InnerText);
                                tmpPrm.y = int.Parse(nodes[i].Attributes["y"].InnerText);
                                tmpPrm.z = int.Parse(nodes[i].Attributes["z"].InnerText);
                                tmpPrm.a = int.Parse(nodes[i].Attributes["a"].InnerText);
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
                System.Windows.Forms.MessageBox.Show("打开轴参数错误");
                return readok = false;
            }
        }
        /// <summary>
        /// 推送到状态栏
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static bool PushToUi_Status(DataGridView view, Image defaultimg)
        {
            //轴的界面初始化
            try
            {
                int index = 0;
                view.AllowUserToAddRows = false;
                foreach (var item in prms)
                {
                    view.Rows.Add();
                    view.Rows[index].Cells[0].Value = item.axisnum;
                    view.Rows[index].Cells[1].Value = item.AxisName;
                    view.Rows[index].Cells[2].Value = "0.00";
                    view.Rows[index].Cells[3].Value = "0.00";
                    view.Rows[index].Cells[4].Value = defaultimg;
                    view.Rows[index].Cells[5].Value = defaultimg;
                    view.Rows[index].Cells[6].Value = defaultimg;
                    view.Rows[index].Cells[7].Value = defaultimg;
                    view.Rows[index].Cells[8].Value = defaultimg;
                    view.Rows[index].Cells[9].Value = item.note;
                    index++;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
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
        public static void PushToUi_Prm(DataGridView view)
        {
            if (view != null)
            {
                view.Rows.Clear();
                view.AllowUserToAddRows = false;
                int line = 0;
                foreach (var item in prms)
                {
                    view.Rows.Add();
                    view.Rows[line].Cells[0].Value = (line + 1).ToString();
                    view.Rows[line].Cells[1].Value = item.Resolution.ToString();
                    view.Rows[line].Cells[2].Value = item.Pith.ToString();
                    view.Rows[line].Cells[3].Value = item.MaxVel.ToString();
                    view.Rows[line].Cells[4].Value = item.MaxAcc.ToString();
                    view.Rows[line].Cells[5].Value = item.HomeType.ToString();
                    view.Rows[line].Cells[6].Value = item.HomeOffset.ToString();
                    view.Rows[line].Cells[7].Value = item.HomeVel1.ToString();
                    view.Rows[line].Cells[8].Value = item.ArriveType.ToString();
                    view.Rows[line].Cells[9].Value = item.ArriveDelay.ToString();
                    view.Rows[line].Cells[10].Value = item.ArrivePand.ToString();
                    line++;
                }
            }
        }
        /// <summary>
        /// 从UI中获取参数
        /// </summary>
        /// <param name="view"></param>
        public static bool PullFromUi_Prm(DataGridView view)
        {
            try
            {
                if (view != null && view.Rows.Count>0)
                {
                    CAxisPrm checkprm = new CAxisPrm();//先验证
                    for (int i = 0; i < view.Rows.Count; i++)
                    {
                        try
                        {
                            checkprm.Resolution = double.Parse((string)view.Rows[i].Cells[1].Value);
                            checkprm.Pith = double.Parse((string)view.Rows[i].Cells[2].Value);
                            checkprm.MaxVel = double.Parse((string)view.Rows[i].Cells[3].Value);
                            checkprm.MaxAcc = double.Parse((string)view.Rows[i].Cells[4].Value);
                            checkprm.HomeType = int.Parse((string)view.Rows[i].Cells[5].Value);
                            checkprm.HomeOffset = double.Parse((string)view.Rows[i].Cells[6].Value);
                            checkprm.HomeVel1 = double.Parse((string)view.Rows[i].Cells[7].Value);
                            checkprm.ArriveType = int.Parse((string)view.Rows[i].Cells[8].Value);
                            checkprm.ArriveDelay = int.Parse((string)view.Rows[i].Cells[9].Value);
                            checkprm.ArrivePand = double.Parse((string)view.Rows[i].Cells[10].Value);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("检测参数错误");
                            return false;
                        }
                    }
                    for (int i = 0; i < view.Rows.Count; i++)
                    {
                        prms[i].Resolution = double.Parse((string)view.Rows[i].Cells[1].Value);
                        prms[i].Pith = double.Parse((string)view.Rows[i].Cells[2].Value);
                        prms[i].MaxVel = double.Parse((string)view.Rows[i].Cells[3].Value);
                        prms[i].MaxAcc = double.Parse((string)view.Rows[i].Cells[4].Value);
                        prms[i].HomeType = int.Parse((string)view.Rows[i].Cells[5].Value);
                        prms[i].HomeOffset = double.Parse((string)view.Rows[i].Cells[6].Value);
                        prms[i].HomeVel1 = double.Parse((string)view.Rows[i].Cells[7].Value);
                        prms[i].ArriveType = int.Parse((string)view.Rows[i].Cells[8].Value);
                        prms[i].ArriveDelay = int.Parse((string)view.Rows[i].Cells[9].Value);
                        prms[i].ArrivePand = double.Parse((string)view.Rows[i].Cells[10].Value);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                MessageBox.Show("检测参数错误");
                return false;
            }
        }
    }
    /// <summary>
    /// 机器类,所有的并行组件都在这边
    /// </summary>
    public class CMachManager : CMachBase,IInitModel
    {
        //所有的机器组件都在这个列表中
        List<CMachBase> machlist = new List<CMachBase>();
        public CMachManager()
        {
            machlist.Clear();
        }
        public void addMach(CMachBase inMach)
        {
            machlist.Add(inMach);
        }
        public override bool preCheckStatus()
        {
            bool statusAll = true;
            foreach (CMachBase item in machlist)
            {
                if (!item.preCheckStatus()) statusAll = false;
            }
            return statusAll;
        }

        public override void Fresh()
        {
            throw new NotImplementedException();
        }

        public override bool Home()
        {
            bool statusAll = true;
            foreach (CMachBase item in machlist)
            {
                if (!item.Home())
                {
                    statusAll = false;
                    break;
                }
            }
            if (!statusAll) Stop();
            return statusAll;
        }

        public bool Init()
        {
            initok = true;
            foreach (CMachBase item in machlist)
            {
                if (item is IInitModel)//判断是否继承了接口
                {
                    if (!((IInitModel)item).Init()) initok = false;
                }
            }
            return initok;
        }

        public override bool Pause()
        {
            throw new NotImplementedException();
        }

        public override bool Reset()
        {
            foreach (CMachBase item in machlist)
            {
                item.Reset();
            }
            return true;
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }

        public override bool Start()
        {
            bool checkOk = true;
            foreach (CMachBase item in machlist)
            {
                if (!item.preCheckStatus())
                {
                    checkOk = false;
                    break;
                }
            }
            if (checkOk)
            {
                foreach (CMachBase item in machlist)
                {
                    item.Start();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Stop()
        {
            foreach (CMachBase item in machlist)
            {
                item.Stop();
            }
            return true;
        }

        public bool UnInit()
        {
            foreach (CMachBase item in machlist)
            {
                if (item is IInitModel)//判断是否继承了接口
                {
                    ((IInitModel)item).UnInit();
                }
            }
            return true;
        }
    }
}
