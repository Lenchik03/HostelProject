using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HostelProject.mvvm.model;
using HostelProject.mvvm.view;
using Type = HostelProject.mvvm.model.Type;

namespace HostelProject.mvvm.viewmodel
{
    public class SettingTypePageVM: BaseVM
    {
        public VmCommand Save { get; set; } // кнопка "Сохранить"

        MainVM mainVM;


        private Type type = new();


        public Type Type // тип, который мы добавляем или редактируем
        {
            get => type;
            set
            {
                type = value;
                Signal();
            }
        }
        public SettingTypePageVM()
        {
            //команда на добавление в базу или обновление типа в базе
            Save = new VmCommand(() =>
            {
                Type.Del = 0;
                if (Type.Id == 0)
                {
                    TypeRepository.Instance.Add(Type); // добавление типа

                }
                else
                {
                    // редактирование выбранного типа
                    TypeRepository.Instance.Update(Type);
                }

                // после успешного добавления или редактирования типа откроется страница менеджера
                mainVM.CurrentPage = new MainPage(mainVM);
            });


        }


        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }

        internal void SetEditType(Type type)
        {
            Type = type;

        }
    }
}
