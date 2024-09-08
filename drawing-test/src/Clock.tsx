export interface ClockProps {
  time: string;
}

export default function Clock({ time }: ClockProps) {
  return (
    <div>
      The current time is {time}{(parseInt(time.charAt(time.length - 1), 10) % 2 == 0 ? " (even)" : null)}.
    </div>
  );
}
