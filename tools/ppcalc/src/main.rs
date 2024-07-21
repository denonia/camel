use clap::Parser;
use rosu_pp::model::mode::GameMode;

#[derive(Parser)]
struct Arguments {
    path: std::path::PathBuf,
    mode: u8,
    mods: u32,
    combo: u32,
    n300: u32,
    n100: u32,
    n50: u32,
    n_katu: u32,
    n_geki: u32,
    n_misses: u32,
    failed: u8,
    passed_objects: u32,
}

fn main() {
    let args = Arguments::parse();

    let map = rosu_pp::Beatmap::from_path(args.path).unwrap();

    let diff_attrs = rosu_pp::Difficulty::new()
        .mods(args.mods)
        .calculate(&map);

    let mut perf_attrs = rosu_pp::Performance::new(diff_attrs)
        .mode_or_ignore(GameMode::from(args.mode))
        .mods(args.mods)
        .combo(args.combo)
        .n300(args.n300)
        .n100(args.n100)
        .n50(args.n50)
        .n_katu(args.n_katu)
        .n_geki(args.n_geki)
        .misses(args.n_misses);

    if args.failed == 1 {
        perf_attrs = perf_attrs.passed_objects(args.passed_objects);
    }

    let pp = perf_attrs.calculate().pp();
    println!("{pp}");
}
