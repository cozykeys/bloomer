import argparse


def add_command_parsers(subparsers: argparse._SubParsersAction) -> None:
    from .expand_vertices import add_expand_vertices_parser

    add_expand_vertices_parser(subparsers)

    from .generate_edges import add_generate_edges_parser

    add_generate_edges_parser(subparsers)

    from .generate_positions import add_generate_positions_parser

    add_generate_positions_parser(subparsers)

    from .generate_traces import add_generate_traces_parser

    add_generate_traces_parser(subparsers)

    from .scratch import add_scratch_parser

    add_scratch_parser(subparsers)
