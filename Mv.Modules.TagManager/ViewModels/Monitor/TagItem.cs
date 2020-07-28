using DataService;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace Mv.Modules.TagManager.ViewModels
{
    public class TagItem : BindableBase
    {

        public TagItem(ITag tag, string tagName, string tagAddr)
        {
            _tag = tag;
            _tagname = tagName;
            _tagValue = _tag.ToString();
            _addr = tagAddr;
            _timestamp = _tag.TimeStamp;
            Description = _tag.GetMetaData().Description;
            _tag.ValueChanged += new ValueChangedEventHandler(TagValueChanged);
        }

        ITag _tag;

        string _tagname;
        public string TagName
        {
            get { return _tagname; }
            set { _tagname = value; }
        }

        string _addr;
        public string Address
        {
            get { return _addr; }
            set { _addr = value; }
        }

        string _tagValue;
        public string TagValue
        {
            get { return _tagValue; }
            set
            {
                SetProperty(ref _tagValue, value);
            }
        }

        DateTime _timestamp;
        public DateTime TimeStamp
        {
            get { return _timestamp; }
            set
            {
                SetProperty(ref _timestamp, value);
            }
        }

        public string Description { get; set; }


        private void TagValueChanged(object sender, ValueChangedEventArgs args)
        {
            TagValue = _tag.ToString();
            TimeStamp = _tag.TimeStamp;
        }

        public int Write(string value)
        {
            if (string.IsNullOrEmpty(value)) return -1;
            if (_tag.Address.VarType == DataType.BOOL)
            {
                if (value == "1") value = "true";
                if (value == "0") value = "false";
            }
            return _tag.Write(value);
        }

        public void SimWrite(string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            Storage stor = Storage.Empty;
            try
            {
                if (_tag.Address.VarType == DataType.STR)
                {
                    ((StringTag)_tag).String = value;
                }
                else
                {
                    stor = _tag.ToStorage(value);
                }
                _tag.Update(stor, DateTime.Now, QUALITIES.QUALITY_GOOD);
            }
            catch { }
        }
       public ObservableCollection<TagItem> TagItems { get; set; } = new ObservableCollection<TagItem>();
        public void Dispose()
        {
            if (_tag != null)
            {
                // ReSharper disable once DelegateSubtraction
               _tag.ValueChanged -= TagValueChanged;
            }
        }
    }
}
