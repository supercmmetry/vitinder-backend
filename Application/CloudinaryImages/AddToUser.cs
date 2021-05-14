using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.CloudinaryImages
{
    public class AddToUser
    {
        public class Command : IRequest
        {
            public User User { get; set; }
            
            public IFormFile File { get; set; }
            
            public string Folder { get; set; }
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
                var currentUser = request.User;

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
                            Transformation = new Transformation()
                                .Width(512)
                                .Height(512)
                                .Crop("fill")
                                .Gravity("face"),
                            Folder = request.Folder
                        });

                        currentUser.ProfileImage = new CloudinaryImage
                        {
                            Id = result.PublicId,
                            Url = result.Uri.ToString()
                        };
                    }

                    using (var stream = request.File.OpenReadStream())
                    {
                        var encoder = new System.Drawing.Blurhash.Encoder();
                        var image = Image.FromStream(stream);
                        currentUser.ProfileImage.BlurHash = encoder.Encode(image, 3, 3);
                    }
                    
                    _context.CloudinaryImages.Add(currentUser.ProfileImage);

                    _context.Users.Update(currentUser);
                    await _context.SaveChangesAsync();
                }
                
                return Unit.Value;
            }
        }
    }
}