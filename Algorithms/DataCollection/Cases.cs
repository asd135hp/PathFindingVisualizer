
namespace Algorithms.DataCollection
{
    public static class Cases
    {
        // more edge cases for better comparison rather than using normal cases
        public static string[] NoSolution = @"
            [15, 25]
            (0,1)
            (17,0) | (19,9)
            (2,0,2,2)
            (8,0,1,2)
            (2,3,1,2)
            (3,4,3,1)
            (9,3,1,1)
            (8,4,2,1)
            (2,8,6,1)
            (3,7,4,1)
            (10,6,2,4)
            (14,0,2,3)
            (14,5,3,3)
            (16,0,1,2)
            (17,1,2,1)
            (18,0,1,1)
            (18,0,1,1)
            (18,8,1,3)
            (19,8,2,1)
            (20,9,1,2)
            (19,10,1,1)
        ".Replace("\t", "").Trim().Split('\n'),
        OneGoalWithObs = @"
            [15,25]
            (1,12)
            (24,0)
            (3,7,10,8)
            (16,3,4,10)
            (2,1,5,4)
            (21,0,3,10)
        ".Replace("\t", "").Trim().Split('\n'),
        TwoGoalsWithObs = @"
            [15,25]
            (1,11)
            (20,7) | (24,0)
            (3,6,10,7)
            (16,3,4,10)
            (2,1,5,4)
            (21,0,3,11)
        ".Replace("\t", "").Trim().Split('\n'),
        ThreeGoalsWithObs = @"
            [15,25]
            (0,0)
            (24,14) | (19,4) | (15,10)
            (1,0,1,10)
            (1,11,1,2)
            (0,14,15,1)
            (3,6,1,7)
            (2,4,3,1)
            (5,6,1,5)
            (5,13,1,2)
            (6,1,1,6)
            (3,2,3,1)
            (2,0,3,1)
            (8,0,1,5)
            (8,6,1,3)
            (7,8,1,6)
            (9,4,4,1)
            (9,6,8,1)
            (14,1,1,6)
            (10,1,1,1)
            (10,2,14,1)
            (12,0,1,1)
            (16,0,1,1)
            (18,1,1,1)
            (20,0,1,1)
            (22,1,1,1)
            (24,0,1,1)
            (22,4,3,1)
            (20,3,1,3)
            (19,5,2,1)
            (16,4,2,1)
            (18,4,1,5)
            (10,8,9,1)
            (10,9,1,2)
            (9,10,1,3)
            (10,12,3,1)
            (12,10,1,2)
            (13,10,2,1)
            (16,9,1,5)
            (14,12,2,1)
            (18,10,1,5)
            (19,10,1,1)
            (20,7,1,4)
            (22,6,1,8)
            (23,6,1,1)
            (24,8,1,1)
            (23,10,1,1)
            (24,12,1,1)
            (20,13,2,1)
        ".Replace("\t", "").Trim().Split('\n'),
        OneGoalWithoutObs = new string[] {
            "[15,25]",
            "(0,1)",
            "(24,7)"
        },
        TwoGoalsWithoutObs = new string[] {
            "[15,25]",
            "(0,0)",
            "(24,14) | (24,0)"
        },
        ThreeGoalsWithoutObs = new string[] {
            "[15,25]",
            "(0,0)",
            "(24,14) | (24,0) | (23,14)"
        };

        public static string[] GetCase(byte goal, bool withObstacle = true)
        {
            if (withObstacle)
                return goal switch
                {
                    0 => NoSolution,
                    1 => OneGoalWithObs,
                    2 => TwoGoalsWithObs,
                    3 => ThreeGoalsWithObs,
                    _ => throw new System.ArgumentException(
                        "Invalid argument provided. Expected goal to be 3 or less"
                    )
                };

            return goal switch
            {
                1 => OneGoalWithoutObs,
                2 => TwoGoalsWithoutObs,
                3 => ThreeGoalsWithoutObs,
                _ => throw new System.ArgumentException(
                    "Invalid argument provided. Expected goal to be less than 3 and not equals to 0"
                )
            };
        }
    }
}
