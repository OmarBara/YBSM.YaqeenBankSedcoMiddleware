using System;
using Core.Domain.Enum;
using Core.Domain.ModelDTO;

namespace Core.Domain.Entities
{
    public class User
    {
        public User(AddUserRequestDto addUserRequestDto, Guid addedBy)
        {
            Email = addUserRequestDto.Email;
            Password = addUserRequestDto.Password;
            PhoneNumber = addUserRequestDto.PhoneNumber;
            UserRole = addUserRequestDto.UserRole;
            AddedBy = addedBy;
        }

        public User()
        {
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Roles UserRole { get; set; }
        public State State { get; set; } = State.Active;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        public Guid AddedBy { get; set; }

        public void UpdateLastLogin()
        {
            LastLogin = DateTime.UtcNow;
        }

        public void EditUser(EditUserRequestDto editUserRequestDto)
        {
            PhoneNumber = editUserRequestDto.PhoneNumber;
            UserRole = editUserRequestDto.UserRole;
        }

        public void UpdateEmail(string email)
        {
            Email = email;
        }

        public void UpdatePassword(string password)
        {
            Password = password;
        }

        public void UpdateState(State state)
        {
            State = state;
        }
    }
}