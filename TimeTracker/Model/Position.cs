using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class Position : T_Position
    {
        public Position GetPosition(int positionid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Positions
                        where d.Id == positionid
                        select new Position()
                        {
                            Id = d.Id,
                            Description = d.Description,
                            Rank = d.Rank,
                            CreateDate = d.CreateDate,
                            LastUpdateDate = d.LastUpdateDate,
                            CreatedBy = d.CreatedBy,
                            LastUpdatedBy = d.LastUpdatedBy
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<Position> GetPosition()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Positions
                        select new Position()
                        {
                            Id = d.Id,
                            Description = d.Description,
                            Rank = d.Rank,
                            CreateDate = d.CreateDate,
                            LastUpdateDate = d.LastUpdateDate,
                            CreatedBy = d.CreatedBy,
                            LastUpdatedBy = d.LastUpdatedBy
                        }).ToList();

            db.Dispose();

            return data;
        }

        public void Insert(Position position)
        {
            T_Position t_position = InsertParse(position);

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_Positions.Add(t_position);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Delete(int id)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_Position t_position = new T_Position();
                    t_position = db.T_Positions.FirstOrDefault(p => p.Id == id);
                    db.T_Positions.Remove(t_position);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(Position position)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_Position t_position = db.T_Positions.FirstOrDefault(p => p.Id == position.Id);
                    UpdateParse(t_position, position);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private T_Position InsertParse(Position position)
        {
            T_Position t_position = new T_Position();
            t_position.Description = position.Description;
            t_position.Rank = position.Rank;
            t_position.CreateDate = position.CreateDate;
            t_position.LastUpdateDate = position.LastUpdateDate;
            t_position.CreatedBy = position.CreatedBy;
            t_position.LastUpdatedBy = position.LastUpdatedBy;

            return t_position;
        }

        private void UpdateParse(T_Position t_position, Position position)
        {
            t_position.Description = position.Description;
            t_position.Rank = position.Rank;
            t_position.LastUpdateDate = position.LastUpdateDate;
            t_position.LastUpdatedBy = position.LastUpdatedBy;
        }
    }
}