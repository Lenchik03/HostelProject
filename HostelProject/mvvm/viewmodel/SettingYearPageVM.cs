using HostelProject.mvvm.model;
using HostelProject.mvvm.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelProject.mvvm.viewmodel
{
    public class SettingYearPageVM: BaseVM
    {
        public VmCommand Save { get; set; } // кнопка "Сохранить"

        MainVM mainVM;


        private Year year = new();


        public Year Year 
        {
            get => year;
            set
            {
                year = value;
                Signal();
            }
        }
        public SettingYearPageVM()
        {
            Save = new VmCommand(() =>
            {
                YearRepository.Instance.AddYear(Year);
                mainVM.CurrentPage = new MainPage(mainVM);
            });


        }


        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }

        internal void SetEditYear(Year year)
        {
            Year = year;

        }
    }
}
