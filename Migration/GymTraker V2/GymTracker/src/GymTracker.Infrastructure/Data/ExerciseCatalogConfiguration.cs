using GymTracker.Domain.Entities;
using GymTracker.Domain.Enums;
using GymTracker.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymTracker.Infrastructure.Data;

public class ExerciseCatalogConfiguration
	: IEntityTypeConfiguration<ExerciseCatalogEntry>
{
	public void Configure(
		EntityTypeBuilder<ExerciseCatalogEntry> builder)
	{
		builder.Property(x => x.Name)
			.HasConversion(name => name.Value, value => new Name(value))
			.HasMaxLength(ExerciseCatalogEntry.MaxNameLength)
			.IsRequired();

		builder.Property(x => x.ExerciseType).HasConversion<string>();
		builder.Property(x => x.DefaultLaterality).HasConversion<string>();

		builder.HasIndex(x => x.Name).IsUnique();

		// 111 ejercicios de DEFAULT_EXERCISES (index.html) con Id determinista 1..111
		builder.HasData(
			new ExerciseCatalogEntry(1, new Name("Press de Banca"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(2, new Name("Press de Banca Inclinado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(3, new Name("Press de Banca Declinado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(4, new Name("Press con Mancuernas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(5, new Name("Press Inclinado con Mancuernas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(6, new Name("Press en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(7, new Name("Press Hammer Strength"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(8, new Name("Chest Press en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(9, new Name("Aperturas con Mancuernas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(10, new Name("Aperturas en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(11, new Name("Aperturas en Poleas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(12, new Name("Peck Deck"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(13, new Name("Press Militar con Barra"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(14, new Name("Press Militar con Mancuernas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(15, new Name("Press Militar en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(16, new Name("Arnold Press"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(17, new Name("Push Press"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(18, new Name("Elevaciones Laterales"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(19, new Name("Elevaciones Laterales en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(20, new Name("Elevaciones Frontales"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(21, new Name("Pájaros (Deltoide Posterior)"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(22, new Name("Reverse Peck Deck"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(23, new Name("Fondos en Paralelas"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(24, new Name("Fondos en Máquina Asistida"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(25, new Name("Fondos en Banco"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(26, new Name("Flexiones"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(27, new Name("Flexiones Diamante"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(28, new Name("Flexiones Declive"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(29, new Name("Extensión de Tríceps en Polea"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(30, new Name("Extensión de Tríceps con Cuerda"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(31, new Name("Extensión de Tríceps en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(32, new Name("Extensión Overhead con Mancuerna"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(33, new Name("Tríceps Francés"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(34, new Name("Press Cerrado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(35, new Name("Dominadas Pronadas"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(36, new Name("Dominadas Supinas (Chin Up)"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(37, new Name("Dominadas Neutras"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(38, new Name("Dominadas en Máquina Asistida"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(39, new Name("Jalón al Pecho"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(40, new Name("Jalón Agarre Cerrado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(41, new Name("Jalón Unilateral"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(42, new Name("Remo con Barra"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(43, new Name("Remo Pendlay"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(44, new Name("Remo con Mancuerna"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(45, new Name("Remo en Polea Baja"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(46, new Name("Remo en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(47, new Name("Remo Hammer Strength"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(48, new Name("Remo T-Bar"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(49, new Name("Remo Alto en Polea"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(50, new Name("Pullover en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(51, new Name("Pullover en Polea"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(52, new Name("Face Pull"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(53, new Name("Curl de Bíceps con Barra"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(54, new Name("Curl de Bíceps con Mancuernas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(55, new Name("Curl Martillo"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(56, new Name("Curl en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(57, new Name("Curl Predicador"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(58, new Name("Curl Scott"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(59, new Name("Curl en Polea"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(60, new Name("Sentadilla Trasera"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(61, new Name("Sentadilla Frontal"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(62, new Name("Sentadilla Goblet"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(63, new Name("Sentadilla Hack"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(64, new Name("Sentadilla Hack Inversa"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(65, new Name("Sentadilla en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(66, new Name("Sentadilla Smith"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(67, new Name("Prensa de Piernas 45°"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(68, new Name("Prensa de Piernas Horizontal"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(69, new Name("Prensa de Piernas Unilateral"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(70, new Name("Peso Muerto Convencional"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(71, new Name("Peso Muerto Sumo"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(72, new Name("Peso Muerto Rumano"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(73, new Name("Peso Muerto en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(74, new Name("Hip Thrust con Barra"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(75, new Name("Hip Thrust en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(76, new Name("Zancadas Caminando"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(77, new Name("Zancadas Estáticas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(78, new Name("Zancadas Reversas"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(79, new Name("Zancadas en Smith"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(80, new Name("Extensiones de Cuádriceps"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(81, new Name("Curl Femoral Acostado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(82, new Name("Curl Femoral Sentado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(83, new Name("Curl Femoral de Pie"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(84, new Name("Aductores en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(85, new Name("Abductores en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(86, new Name("Elevación de Gemelos de Pie"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(87, new Name("Elevación de Gemelos Sentado"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(88, new Name("Gemelos en Prensa"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(89, new Name("Crunch Abdominal"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(90, new Name("Crunch en Máquina"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(91, new Name("Crunch en Polea"), ExerciseType.Standard, Laterality.Bilateral),
			new ExerciseCatalogEntry(92, new Name("Crunch Declinado"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(93, new Name("Plancha Abdominal"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(94, new Name("Plancha Lateral"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(95, new Name("Plancha con Peso"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(96, new Name("Elevaciones de Piernas"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(97, new Name("Elevaciones de Piernas Colgado"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(98, new Name("Ab Wheel"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(99, new Name("Saltar Cuerda"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(100, new Name("Burpees"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(101, new Name("Jump Squats"), ExerciseType.Bodyweight, Laterality.Bilateral),
			new ExerciseCatalogEntry(102, new Name("Caminadora"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(103, new Name("Bicicleta Estática"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(104, new Name("Bicicleta Reclinada"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(105, new Name("Elíptica"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(106, new Name("Escaladora"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(107, new Name("Remo Ergómetro"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(108, new Name("Assault Bike"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(109, new Name("Wall Sit"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(110, new Name("Dead Hang"), ExerciseType.Time, Laterality.Bilateral),
			new ExerciseCatalogEntry(111, new Name("Farmer Walk"), ExerciseType.Time, Laterality.Bilateral)
		);
	}
}
