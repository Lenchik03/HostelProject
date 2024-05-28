using HostelProject.mvvm.model;
using HostelProject.mvvm.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Type = HostelProject.mvvm.model.Type;

namespace HostelProject.mvvm.viewmodel
{
    public class SettingRoomPageVM: BaseVM
    {
        private Capacity selectedCapacity;
        private Type selectedType;
        public VmCommand Save { get; set; } // кнопка "Сохранить"
        MainVM mainVM;

        private Room room = new();

        public Room Room // номер, который мы добавляем или редактируем
        {
            get => room;
            set
            {
                room = value;
                Signal();
            }
        }

        public Capacity SelectedCapacity // выбранное кол-во мест в номере из ComboBox`a
        {
            get => selectedCapacity; set
            {
                selectedCapacity = value;
                Signal();
            }
        }
        public Type SelectedType // выбранный тип из ComboBox`a
        {
            get => selectedType; set
            {
                selectedType = value;
                Signal();
            }
        }

        
        public List<Capacity> AllCapacity { get; set; } // список мест (ComboBox абонментов)
        public List<Type> AllType { get; set; } // список типов (ComboBox видов)

        public SettingRoomPageVM()
        {
            // получение списка мест
            AllCapacity = (List<Capacity>?)CapacityRepository.Instance.GetAllCapacities();

            // получение списка типов
            AllType = (List<Type>?)TypeRepository.Instance.GetAllTypes();


            //команда на добавление в базу или обновление гостя в базе
            Save = new VmCommand(() => {

                // если из Combobox`а НЕ выбрано кол-во мест, то по умолчанию будет выбран одноместный
                Room.CapacityId = SelectedCapacity?.Id ?? 1;

                // если из Combobox`а НЕ выбран тип, то по умолчанию будет выбран первый вид
                Room.TypeId = SelectedType?.Id ?? 1;

                Room.Del = 0; // по умолчанию номер не удален
                
                if (Room.Id == 0)
                {
                    if(Room.CapacityId == 1)
                        Room.Capacity = 1;
                    else if (Room.CapacityId == 2)
                        Room.Capacity = 2;
                    else
                        Room.Capacity = 4;

                    RoomRepository.Instance.Add(Room); // добавление номер

                }
                else
                {
                    RoomRepository.Instance.Update(Room); // если номер выбран из списка - редактирование номера
                }

                mainVM.CurrentPage = new MainPage(mainVM); // после успешного добавления или редактирования номера, откроется главное меню
            });


        }


        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }

        internal void SetEditRoom(Room selectedRoom)
        {
            Room = selectedRoom;
            SelectedCapacity = AllCapacity.FirstOrDefault(s => s.Id == room.CapacityId);
            SelectedType = AllType.FirstOrDefault(s => s.Id == room.TypeId);
        }
    }
}
