using Application.Tools;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Core.Interfaces.Services;

namespace Application.Services;

public class CardService: ICardService
{
    private readonly ICardRepository _cardRepository;
    private readonly EncryptionHelper _encryptionHelper;
    
    public CardService(ICardRepository cardRepository, EncryptionHelper encryptionHelper)
    {
        _cardRepository = cardRepository;
        _encryptionHelper = encryptionHelper;
    }
    
    public async Task<IEnumerable<Card>> GetByUserId(int userId)
    {
        var cards = await _cardRepository.GetByUserId(userId);
        foreach (var card in cards)
        {
            card.pan = _encryptionHelper.Decrypt(card.pan);
            card.cvv = _encryptionHelper.Decrypt(card.cvv);
            card.expiration_date = _encryptionHelper.Decrypt(card.expiration_date);
        }
        return cards;
    }

    public async Task<Card> GetById(int id)
    {
        var card = await _cardRepository.GetById(id);
        if (card != null)
        {
            card.pan = _encryptionHelper.Decrypt(card.pan);
            card.cvv = _encryptionHelper.Decrypt(card.cvv);
            card.expiration_date = _encryptionHelper.Decrypt(card.expiration_date);
        }
        return card;
    }

    public async Task<Card> Create(CreateCardDto dto)
    {
        dto.pan = _encryptionHelper.Encrypt(dto.pan);
        
        var existingCards = await _cardRepository.GetByUserId(dto.user_id);
        foreach (var card in existingCards)
        {
            if (card.pan == dto.pan)
                throw new Exception("Card already exists");
        }
        
        dto.cvv = _encryptionHelper.Encrypt(dto.cvv);
        dto.expiration_date = _encryptionHelper.Encrypt(dto.expiration_date);
        
        return await _cardRepository.Create(dto);
    }

    public async Task Update(UpdateCardDto dto, int id)
    {
        dto.pan = dto.pan != null? _encryptionHelper.Encrypt(dto.pan): null;
        dto.cvv = dto.cvv != null? _encryptionHelper.Encrypt(dto.cvv): null;
        dto.expiration_date = dto.expiration_date != null? _encryptionHelper.Encrypt(dto.expiration_date): null;
        await _cardRepository.Update(dto, id);
    }

    public async Task ChangeCardStatus(ChangeCardStatusDto dto, int id)
    {
        await _cardRepository.ChangeCardStatus(id, dto.status);
    }

    public async Task Delete(int id)
    {
        await _cardRepository.Delete(id);
    }
}