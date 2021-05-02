using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CloudinaryImages
{
    public class AddToUser
    {
        public class Command : IRequest
        {
            public string UserId { get; set; }
            
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly Cloudinary _cloudinary;

            public Handler(DataContext context, Cloudinary cloudinary)
            {
                _context = context;
                _cloudinary = cloudinary;
            }
            
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var currentUser = await _context.Users
                    .Include(user => user.ProfileImage)
                    .Where(user => user.Id == request.UserId)
                    .FirstAsync();

                if (currentUser.ProfileImage != null)
                {
                    await _cloudinary.DestroyAsync(new DeletionParams(currentUser.ProfileImage.Id));
                    _context.CloudinaryImages.Remove(currentUser.ProfileImage);
                    await _context.SaveChangesAsync();
                }

                if (request.File.Length > 0)
                {
                    using (var stream = request.File.OpenReadStream())
                    {
                        var result = await _cloudinary.UploadAsync(new ImageUploadParams
                        {
                            File = new FileDescription(request.File.Name, stream),
                            Transformation = new Transformation()           //  *
                                .Width(512).Height(512)
                                .Crop("fill")
                                .Gravity("face")
                        });

                        currentUser.ProfileImage = new CloudinaryImage
                        {
                            Id = result.PublicId,
                            Url = result.Uri.ToString()
                        };

                        _context.CloudinaryImages.Add(currentUser.ProfileImage);

                        _context.Users.Update(currentUser);
                        await _context.SaveChangesAsync();
                    }
                }
                
                return Unit.Value;
            }
        }
    }
}