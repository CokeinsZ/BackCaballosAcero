using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.RepositoriesInterfaces;

public interface ICardRepository
{
    Task<IEnumerable<Card>> GetByUserId(int userId);
    Task<Card?> GetById(int id);
    Task<Card> Create(CreateCardDto dto);
    Task Update(UpdateCardDto dto, int id);
    Task ChangeCardStatus(int id, string status);
    Task Delete(int id);
}