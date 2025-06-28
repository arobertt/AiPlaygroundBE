using AiPlayground.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiPlayground.DataAccess.Configurations
{
    public class PromptConfiguration : IEntityTypeConfiguration<Prompt>
    {
        public void Configure(EntityTypeBuilder<Prompt> builder)
        {
            builder.ToTable("Prompt")
                .HasKey(p => p.Id);

            builder.HasOne(p => p.Scope)
                .WithMany(s => s.Prompts)
                .HasForeignKey(p => p.ScopeId)
                .HasConstraintName("FK_Prompt_Scope");
        }
    }
}
