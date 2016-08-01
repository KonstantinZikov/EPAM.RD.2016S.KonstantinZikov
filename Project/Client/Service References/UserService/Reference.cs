﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.UserService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="User", Namespace="http://schemas.datacontract.org/2004/07/Entities")]
    [System.SerializableAttribute()]
    public partial class User : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime DateOfBirthField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FirstNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Client.UserService.Gender GenderField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LastNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PersonalIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Client.UserService.VisaRecord[] VisaRecordsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime DateOfBirth {
            get {
                return this.DateOfBirthField;
            }
            set {
                if ((this.DateOfBirthField.Equals(value) != true)) {
                    this.DateOfBirthField = value;
                    this.RaisePropertyChanged("DateOfBirth");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FirstName {
            get {
                return this.FirstNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FirstNameField, value) != true)) {
                    this.FirstNameField = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Client.UserService.Gender Gender {
            get {
                return this.GenderField;
            }
            set {
                if ((this.GenderField.Equals(value) != true)) {
                    this.GenderField = value;
                    this.RaisePropertyChanged("Gender");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastName {
            get {
                return this.LastNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LastNameField, value) != true)) {
                    this.LastNameField = value;
                    this.RaisePropertyChanged("LastName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PersonalId {
            get {
                return this.PersonalIdField;
            }
            set {
                if ((object.ReferenceEquals(this.PersonalIdField, value) != true)) {
                    this.PersonalIdField = value;
                    this.RaisePropertyChanged("PersonalId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Client.UserService.VisaRecord[] VisaRecords {
            get {
                return this.VisaRecordsField;
            }
            set {
                if ((object.ReferenceEquals(this.VisaRecordsField, value) != true)) {
                    this.VisaRecordsField = value;
                    this.RaisePropertyChanged("VisaRecords");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Gender", Namespace="http://schemas.datacontract.org/2004/07/Entities")]
    public enum Gender : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Male = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Female = 1,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="VisaRecord", Namespace="http://schemas.datacontract.org/2004/07/Entities")]
    [System.SerializableAttribute()]
    public partial struct VisaRecord : System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CountryField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime EndsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime StartsField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Country {
            get {
                return this.CountryField;
            }
            set {
                if ((object.ReferenceEquals(this.CountryField, value) != true)) {
                    this.CountryField = value;
                    this.RaisePropertyChanged("Country");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Ends {
            get {
                return this.EndsField;
            }
            set {
                if ((this.EndsField.Equals(value) != true)) {
                    this.EndsField = value;
                    this.RaisePropertyChanged("Ends");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Starts {
            get {
                return this.StartsField;
            }
            set {
                if ((this.StartsField.Equals(value) != true)) {
                    this.StartsField = value;
                    this.RaisePropertyChanged("Starts");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UserService.IWcfUserService")]
    public interface IWcfUserService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Add", ReplyAction="http://tempuri.org/IUserService/AddResponse")]
        int Add(Client.UserService.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Add", ReplyAction="http://tempuri.org/IUserService/AddResponse")]
        System.Threading.Tasks.Task<int> AddAsync(Client.UserService.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Delete", ReplyAction="http://tempuri.org/IUserService/DeleteResponse")]
        void Delete(Client.UserService.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Delete", ReplyAction="http://tempuri.org/IUserService/DeleteResponse")]
        System.Threading.Tasks.Task DeleteAsync(Client.UserService.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfUserService/SearchById", ReplyAction="http://tempuri.org/IWcfUserService/SearchByIdResponse")]
        Client.UserService.User[] SearchById(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfUserService/SearchById", ReplyAction="http://tempuri.org/IWcfUserService/SearchByIdResponse")]
        System.Threading.Tasks.Task<Client.UserService.User[]> SearchByIdAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfUserService/SearchByFirstName", ReplyAction="http://tempuri.org/IWcfUserService/SearchByFirstNameResponse")]
        Client.UserService.User[] SearchByFirstName(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfUserService/SearchByFirstName", ReplyAction="http://tempuri.org/IWcfUserService/SearchByFirstNameResponse")]
        System.Threading.Tasks.Task<Client.UserService.User[]> SearchByFirstNameAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfUserService/SearchByLastName", ReplyAction="http://tempuri.org/IWcfUserService/SearchByLastNameResponse")]
        Client.UserService.User[] SearchByLastName(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfUserService/SearchByLastName", ReplyAction="http://tempuri.org/IWcfUserService/SearchByLastNameResponse")]
        System.Threading.Tasks.Task<Client.UserService.User[]> SearchByLastNameAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfUserService/SearchByGender", ReplyAction="http://tempuri.org/IWcfUserService/SearchByGenderResponse")]
        Client.UserService.User[] SearchByGender(Client.UserService.Gender gender);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfUserService/SearchByGender", ReplyAction="http://tempuri.org/IWcfUserService/SearchByGenderResponse")]
        System.Threading.Tasks.Task<Client.UserService.User[]> SearchByGenderAsync(Client.UserService.Gender gender);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfUserService/SearchByDateOfBirth", ReplyAction="http://tempuri.org/IWcfUserService/SearchByDateOfBirthResponse")]
        Client.UserService.User[] SearchByDateOfBirth(System.DateTime date);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfUserService/SearchByDateOfBirth", ReplyAction="http://tempuri.org/IWcfUserService/SearchByDateOfBirthResponse")]
        System.Threading.Tasks.Task<Client.UserService.User[]> SearchByDateOfBirthAsync(System.DateTime date);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWcfUserServiceChannel : Client.UserService.IWcfUserService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WcfUserServiceClient : System.ServiceModel.ClientBase<Client.UserService.IWcfUserService>, Client.UserService.IWcfUserService {
        
        public WcfUserServiceClient() {
        }
        
        public WcfUserServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WcfUserServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WcfUserServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WcfUserServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Add(Client.UserService.User user) {
            return base.Channel.Add(user);
        }
        
        public System.Threading.Tasks.Task<int> AddAsync(Client.UserService.User user) {
            return base.Channel.AddAsync(user);
        }
        
        public void Delete(Client.UserService.User user) {
            base.Channel.Delete(user);
        }
        
        public System.Threading.Tasks.Task DeleteAsync(Client.UserService.User user) {
            return base.Channel.DeleteAsync(user);
        }
        
        public Client.UserService.User[] SearchById(int id) {
            return base.Channel.SearchById(id);
        }
        
        public System.Threading.Tasks.Task<Client.UserService.User[]> SearchByIdAsync(int id) {
            return base.Channel.SearchByIdAsync(id);
        }
        
        public Client.UserService.User[] SearchByFirstName(string name) {
            return base.Channel.SearchByFirstName(name);
        }
        
        public System.Threading.Tasks.Task<Client.UserService.User[]> SearchByFirstNameAsync(string name) {
            return base.Channel.SearchByFirstNameAsync(name);
        }
        
        public Client.UserService.User[] SearchByLastName(string name) {
            return base.Channel.SearchByLastName(name);
        }
        
        public System.Threading.Tasks.Task<Client.UserService.User[]> SearchByLastNameAsync(string name) {
            return base.Channel.SearchByLastNameAsync(name);
        }
        
        public Client.UserService.User[] SearchByGender(Client.UserService.Gender gender) {
            return base.Channel.SearchByGender(gender);
        }
        
        public System.Threading.Tasks.Task<Client.UserService.User[]> SearchByGenderAsync(Client.UserService.Gender gender) {
            return base.Channel.SearchByGenderAsync(gender);
        }
        
        public Client.UserService.User[] SearchByDateOfBirth(System.DateTime date) {
            return base.Channel.SearchByDateOfBirth(date);
        }
        
        public System.Threading.Tasks.Task<Client.UserService.User[]> SearchByDateOfBirthAsync(System.DateTime date) {
            return base.Channel.SearchByDateOfBirthAsync(date);
        }
    }
}
