using Modsen.Domain;

namespace Modsen.Application
{
public class GetAllUsersUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllUsersUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<User>> Execute(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (page <= 0 || pageSize <= 0)
        {
            throw new BadRequestException("Page and pageSize must be greater than zero.");
        }

        var usersQuery = await _unitOfWork.UserRepository.GetUsersAsync(page, pageSize);

        var users = await _unitOfWork.UserRepository.GetUsersAsync(page, pageSize);

        if (!users.Any())
        {
            throw new NotFoundException("No users found.");
        }

        return users;
    }

}

}