using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services;

public interface ICardService
{
    Task<IEnumerable<Card>> GetByUserId(int userId);
    Task<Card> GetById(int id);
    Task<Card> Create(CreateCardDto dto);
    Task Update(UpdateCardDto dto, int id);
    Task ChangeCardStatus(ChangeCardStatusDto dto, int id);
    Task Delete(int id);
}