using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Database;

namespace Infrastructure.Repositories;

public class ImageRepository(MessengerContext context) : Repository<Image>(context), IImageRepository
{
}