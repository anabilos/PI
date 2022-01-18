using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OzoMvc.Models
{
    public partial class PI05Context : DbContext
    {
       
        public PI05Context(DbContextOptions<PI05Context> options)
            : base(options)
        {
        }

        public virtual DbSet<CertifikatZaposlenik> CertifikatZaposlenik { get; set; }
        public virtual DbSet<Certifikati> Certifikati { get; set; }
        public virtual DbSet<Grad> Grad { get; set; }
        public virtual DbSet<KategorijaPoslova> KategorijaPoslova { get; set; }
        public virtual DbSet<KategorijaZaposlenik> KategorijaZaposlenik { get; set; }
        public virtual DbSet<Klijent> Klijent { get; set; }
        public virtual DbSet<Mjesto> Mjesto { get; set; }
        public virtual DbSet<Natjecaj> Natjecaj { get; set; }
        public virtual DbSet<NatjecajPonudjivac> NatjecajPonudjivac { get; set; }
        public virtual DbSet<Normativi> Normativi { get; set; }
        public virtual DbSet<Oprema> Oprema { get; set; }
        public virtual DbSet<Ponudjivaci> Ponudjivaci { get; set; }
        public virtual DbSet<Posao> Posao { get; set; }
        public virtual DbSet<PosaoOprema> PosaoOprema { get; set; }
        public virtual DbSet<ReferentniTip> ReferentniTip { get; set; }
        public virtual DbSet<Skladište> Skladište { get; set; }
        public virtual DbSet<Transakcija> Transakcija { get; set; }
        public virtual DbSet<Usluga> Usluga { get; set; }
        public virtual DbSet<UslugaKategorija> UslugaKategorija { get; set; }
        public virtual DbSet<UslugaVrstaOpreme> UslugaVrstaOpreme { get; set; }
        public virtual DbSet<VrstaOpreme> VrstaOpreme { get; set; }
        public virtual DbSet<Zaposlenik> Zaposlenik { get; set; }
        public virtual DbSet<ZaposlenikPosao> ZaposlenikPosao { get; set; }
       




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
           modelBuilder.Entity<CertifikatZaposlenik>(entity =>
            {
                entity.ToTable("certifikat_zaposlenik");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CertifikatId).HasColumnName("certifikat_id");

                entity.Property(e => e.ZaposlenikId).HasColumnName("zaposlenik_id");
               
                entity.HasOne(d => d.Certifikat)
                    .WithMany(p => p.CertifikatZaposlenik)
                    .HasForeignKey(d => d.CertifikatId)
                    .HasConstraintName("FK_certifikat_zaposlenik_certifikati");

                entity.HasOne(d => d.Zaposlenik)
                    .WithMany(p => p.CertifikatZaposlenik)
                    .HasForeignKey(d => d.ZaposlenikId)
                    .HasConstraintName("FK_certifikat_zaposlenik_zaposlenik");
            });

            modelBuilder.Entity<Certifikati>(entity =>
            {
                entity.ToTable("certifikati");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Naziv)
                    .HasColumnName("naziv")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Opis)
                    .HasColumnName("opis")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<Grad>(entity =>
            {
                entity.ToTable("grad");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Naziv)
                    .HasColumnName("naziv")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<KategorijaPoslova>(entity =>
            {
                entity.ToTable("kategorija_poslova");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Naziv)
                    .HasColumnName("naziv")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<KategorijaZaposlenik>(entity =>
            {
                entity.ToTable("kategorija_zaposlenik");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.KategorijaId).HasColumnName("kategorija_id");

                entity.Property(e => e.ZaposlenikId).HasColumnName("zaposlenik_id");

                entity.HasOne(d => d.Kategorija)
                    .WithMany(p => p.KategorijaZaposlenik)
                    .HasForeignKey(d => d.KategorijaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_kategorija_zaposlenik_kategorija_poslova");

                entity.HasOne(d => d.Zaposlenik)
                    .WithMany(p => p.KategorijaZaposlenik)
                    .HasForeignKey(d => d.ZaposlenikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_kategorija_zaposlenik_zaposlenik");
            });

            modelBuilder.Entity<Klijent>(entity =>
            {
                entity.ToTable("klijent");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Naziv)
                    .HasColumnName("naziv")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Mjesto>(entity =>
            {
                entity.ToTable("mjesto");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                    

                entity.Property(e => e.GradId).HasColumnName("grad_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("naziv")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Grad)
                    .WithMany(p => p.Mjesto)
                    .HasForeignKey(d => d.GradId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mjesto_grad");
            });

            modelBuilder.Entity<Natjecaj>(entity =>
            {
                entity.ToTable("natjecaj");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasColumnName("opis")
                    .HasColumnType("text");

                entity.Property(e => e.TrajanjeDo)
                    .HasColumnName("trajanje_do")
                    .HasColumnType("date");

                entity.Property(e => e.TrajanjeOd)
                    .HasColumnName("trajanje_od")
                    .HasColumnType("date");

                entity.Property(e => e.UslugaId).HasColumnName("usluga_id");

                entity.HasOne(d => d.Usluga)
                    .WithMany(p => p.Natjecaj)
                    .HasForeignKey(d => d.UslugaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_natjecaj_usluga");
            });

            modelBuilder.Entity<NatjecajPonudjivac>(entity =>
            {
                entity.ToTable("natjecaj_ponudjivac");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DobivenNatjecaj).HasColumnName("dobiven_natjecaj");

                entity.Property(e => e.NatjecajId).HasColumnName("natjecaj_id");

                entity.Property(e => e.PonudjivacId).HasColumnName("ponudjivac_id");
                entity.Property(e => e.CijenaPonude).HasColumnName("cijena_ponude");

                entity.HasOne(d => d.Natjecaj)
                    .WithMany(p => p.NatjecajPonudjivac)
                    .HasForeignKey(d => d.NatjecajId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_natjecaj_ponudjivac_natjecaj");

                entity.HasOne(d => d.Ponudjivac)
                    .WithMany(p => p.NatjecajPonudjivac)
                    .HasForeignKey(d => d.PonudjivacId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_natjecaj_ponudjivac_ponudjivaci");
            });

            modelBuilder.Entity<Normativi>(entity =>
            {
                entity.ToTable("normativi");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Opis)
                    .HasColumnName("opis")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<Oprema>(entity =>
            {
                entity.HasKey(e => e.InventarniBroj);

                entity.ToTable("oprema");

                entity.Property(e => e.InventarniBroj)
                    .HasColumnName("inventarni_broj");
                   
                entity.Property(e => e.KnjigovodstvenaVrijednost).HasColumnName("knjigovodstvena_vrijednost");

                entity.Property(e => e.KupovnaVrijednost).HasColumnName("kupovna_vrijednost");

                entity.Property(e => e.Naziv)
                    .HasColumnName("naziv")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ReferentniId).HasColumnName("referentni_id");

                entity.Property(e => e.SkladisteId).HasColumnName("skladiste_id");

                entity.Property(e => e.VrijemeAmortizacije).HasColumnName("vrijeme_amortizacije");

                entity.Property(e => e.VrstaId).HasColumnName("vrsta_id");

                entity.HasOne(d => d.Referentni)
                    .WithMany(p => p.Oprema)
                    .HasForeignKey(d => d.ReferentniId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_oprema_referentni_tip");

                entity.HasOne(d => d.Skladiste)
                    .WithMany(p => p.Oprema)
                    .HasForeignKey(d => d.SkladisteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_oprema_skladište");

                entity.HasOne(d => d.Vrsta)
                    .WithMany(p => p.Oprema)
                    .HasForeignKey(d => d.VrstaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_oprema_vrsta_opreme");
            });

            modelBuilder.Entity<Ponudjivaci>(entity =>
            {
                entity.ToTable("ponudjivaci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("naziv")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Posao>(entity =>
            {
                entity.ToTable("posao");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.MjestoId).HasColumnName("mjesto_id");

                entity.Property(e => e.Troskovi).HasColumnName("troskovi");

                entity.Property(e => e.UslugaId).HasColumnName("usluga_id");

                entity.Property(e => e.Vrijeme)
                    .HasColumnName("vrijeme")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.UslugaNavigation)
                    .WithMany(p => p.Posao)
                    .HasForeignKey(d => d.UslugaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_posao_usluga");
            });

            modelBuilder.Entity<PosaoOprema>(entity =>
            {
                entity.ToTable("posao_oprema");

                entity.Property(e => e.Id)
                    .HasColumnName("id").ValueGeneratedOnAdd();
                   

                entity.Property(e => e.OpremaId).HasColumnName("oprema_id");

                entity.Property(e => e.PosaoId)
                    .HasColumnName("posao_id");
                    

                entity.Property(e => e.Satnica).HasColumnName("satnica");

                entity.HasOne(d => d.Oprema)
                    .WithMany(p => p.PosaoOprema)
                    .HasForeignKey(d => d.OpremaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_posao_oprema_oprema");

                entity.HasOne(d => d.Posao)
                    .WithMany(p => p.PosaoOprema)
                    .HasForeignKey(d => d.PosaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_posao_oprema_posao");
            });

            modelBuilder.Entity<ReferentniTip>(entity =>
            {
                entity.ToTable("referentni_tip");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                    

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Naziv)
                    .HasColumnName("naziv")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Skladište>(entity =>
            {
                entity.ToTable("skladište");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                   

                entity.Property(e => e.MjestoId).HasColumnName("mjesto_id");
                entity.Property(e => e.Naziv).HasColumnName("naziv");

                entity.HasOne(d => d.Mjesto)
                    .WithMany(p => p.Skladište)
                    .HasForeignKey(d => d.MjestoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_skladište_mjesto");
            });

            modelBuilder.Entity<Transakcija>(entity =>
            {
                entity.ToTable("transakcija");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Datum)
                    .HasColumnName("datum")
                    .HasColumnType("date");

                entity.Property(e => e.KlijentId).HasColumnName("klijent_id");

                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasColumnName("opis")
                    .HasColumnType("text");

                entity.Property(e => e.OpremaId).HasColumnName("oprema_id");

                entity.HasOne(d => d.Klijent)
                    .WithMany(p => p.Transakcija)
                    .HasForeignKey(d => d.KlijentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_transakcija_klijent");

                entity.HasOne(d => d.Oprema)
                    .WithMany(p => p.Transakcija)
                    .HasForeignKey(d => d.OpremaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_transakcija_oprema");
            });

            modelBuilder.Entity<Usluga>(entity =>
            {
                entity.ToTable("usluga");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                    

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("naziv")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.NormativId).HasColumnName("normativ_id");

                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasColumnName("opis")
                    .HasColumnType("text");

                entity.HasOne(d => d.Normativ)
                    .WithMany(p => p.Usluga)
                    .HasForeignKey(d => d.NormativId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_usluga_normativi");
            });

            modelBuilder.Entity<UslugaKategorija>(entity =>
            {
                entity.ToTable("usluga_kategorija");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                    

                entity.Property(e => e.Kategorija).HasColumnName("kategorija");

                entity.Property(e => e.UslugaId)
                    .HasColumnName("usluga_id");
                    

                entity.HasOne(d => d.KategorijaNavigation)
                    .WithMany(p => p.UslugaKategorija)
                    .HasForeignKey(d => d.Kategorija)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_usluga_kategorija_kategorija_poslova");

                entity.HasOne(d => d.Usluga)
                    .WithMany(p => p.UslugaKategorija)
                    .HasForeignKey(d => d.UslugaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_usluga_kategorija_usluga");
            });

            modelBuilder.Entity<UslugaVrstaOpreme>(entity =>
            {
                entity.ToTable("usluga_vrsta_opreme");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UslugaId).HasColumnName("usluga_id");

                entity.Property(e => e.VrstaId).HasColumnName("vrsta_id");

                entity.HasOne(d => d.Usluga)
                    .WithMany(p => p.UslugaVrstaOpreme)
                    .HasForeignKey(d => d.UslugaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_usluga_vrsta_opreme_usluga");

                entity.HasOne(d => d.Vrsta)
                    .WithMany(p => p.UslugaVrstaOpreme)
                    .HasForeignKey(d => d.VrstaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_usluga_vrsta_opreme_vrsta_opreme");
            });

            modelBuilder.Entity<VrstaOpreme>(entity =>
            {
                entity.ToTable("vrsta_opreme");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                    

                entity.Property(e => e.Naziv)
                    .HasColumnName("naziv")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Zaposlenik>(entity =>
            {
                entity.ToTable("zaposlenik");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.DatumRodjenja)
                    .HasColumnName("datum_rodjenja")
                    .HasColumnType("date");

                entity.Property(e => e.Ime)
                    .HasColumnName("ime")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.MjesecniTrosak).HasColumnName("mjesecni_trosak");

                entity.Property(e => e.MjestoId).HasColumnName("mjesto_id");

                entity.Property(e => e.Prezime)
                    .HasColumnName("prezime")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Mjesto)
                    .WithMany(p => p.Zaposlenik)
                    .HasForeignKey(d => d.MjestoId)
                    .HasConstraintName("FK_zaposlenik_mjesto");
            });

            modelBuilder.Entity<ZaposlenikPosao>(entity =>
            {
                entity.ToTable("zaposlenik_posao");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                   

                entity.Property(e => e.PosaoId).HasColumnName("posao_id");

                entity.Property(e => e.Satnica).HasColumnName("satnica");

                entity.Property(e => e.ZaposlenikId).HasColumnName("zaposlenik_id");

                entity.HasOne(d => d.Posao)
                    .WithMany(p => p.ZaposlenikPosao)
                    .HasForeignKey(d => d.PosaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_zaposlenik_posao_posao");

                entity.HasOne(d => d.ZaposlenikNavigation)
                    .WithMany(p => p.ZaposlenikPosao)
                    .HasForeignKey(d => d.ZaposlenikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_zaposlenik_posao_zaposlenik");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
