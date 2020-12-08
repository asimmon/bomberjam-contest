namespace Bomberjam
{
    internal interface IToDto<out To>
    {
        public To Convert();
    }
}