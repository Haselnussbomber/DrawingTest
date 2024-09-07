export interface ClockProps {
  id?: string;
  time: string;
}

export default function Clock({ id, time }: ClockProps) {
  return <div id={id} style={{ color: '#f90' }}>The current time is {time}{(parseInt(time.charAt(time.length - 1), 10) % 2 == 0 ? " (even)" : null)}.</div>;
}
