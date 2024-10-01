using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;
using First_Sample.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Persistence.Repositories
{
    public class SiteSettingRepository : ISiteSettingRepository
    {
        private readonly First_Sample_Context _context;
        public SiteSettingRepository(First_Sample_Context context)
        {
            _context = context;  
        }


        public async Task<bool> Add(SiteSetting entity)
        {
            try
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;// اگر رکوردی اضافه شد، true برگردانید.
            }
            catch (Exception)
            {
                // در صورت بروز خطا، false برگردانید.
                return false;
            }
        }

        public async Task<IReadOnlyList<SiteSetting>> GetAll()
        {
            var settings = await _context.SiteSettings.ToListAsync();
            return settings;
        }

        public async Task<SiteSetting> GetKey(string id)
        {
            var guid = await _context.SiteSettings.FirstOrDefaultAsync
                (o=>o.Key==id);   
            return guid == null ? null : guid;
        }

        public async Task<bool> Update(SiteSetting entity)
        {
            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return true; // اگر رکوردی اضافه شد، true برگردانید.
            }
            catch (Exception)
            {
                // در صورت بروز خطا، false برگردانید.
                return false;
            }
        }
    }
}
