//using Contacts.BL;
//using Contacts.MVVM.Framework;
//using Contacts.ViewModels.Messages;
//using Contacts.ViewModels.Models;
//using System;
//using System.Windows.Input;

//namespace Contacts.ViewModels.Commands
//{
//    public class SaveContactCommand : ICommand
//    {
//        private readonly IContactsService contactsService;
//        private readonly IMessenger messenger;

//        public event EventHandler CanExecuteChanged;

//        public SaveContactCommand(IContactsService contactsService, IMessenger messenger)
//        {
//            this.contactsService = contactsService;
//            this.messenger = messenger;
//        }

//        public bool CanExecute(object parameter)
//        {
//            return true;
//        }

//        public async void Execute(object parameter)
//        {
//            var contact = parameter as ContactModel;
//            contact = await contactsService.Save(contact);
//            await messenger.Send(new ContactEditedMessage(contact));
//        }
//    }
//}